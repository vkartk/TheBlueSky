using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TheBlueSky.Auth.DTOs.Requests;
using TheBlueSky.Auth.DTOs.Responses;
using TheBlueSky.Auth.Models;

namespace TheBlueSky.Auth.Services
{
    public class AuthTokenService : IAuthTokenService
    {

        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _context; 

        public AuthTokenService(IConfiguration configuration, UserManager<ApplicationUser> userManager, AuthDbContext context)
        {
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
        }

        private async Task<(JwtSecurityToken token, string tokenId)> CreateJwtToken(ApplicationUser user)
        {
            var tokenId = Guid.NewGuid().ToString();
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, tokenId),
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var expiry = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JWT:ExpiryInMinutes"]));
            var signingCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: expiry,
                claims: authClaims,
                signingCredentials: signingCredentials
            );

            return (token, tokenId);
        }

        private async Task<string> CreateAndSaveRefreshToken(string jwtId, string userId)
        {
            var refreshExpiryMonths = 1;
            var configuredValue = _configuration["JWT:RefreshExpiryInMonths"];
            int.TryParse(configuredValue, out refreshExpiryMonths);

            var refreshToken = new RefreshToken
            {
                JwtId = jwtId,
                UserId = userId,
                Token = Guid.NewGuid().ToString("N"),
                IsUsed = false,
                IsRevoked = false,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(refreshExpiryMonths)
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }


        public async Task<LoginResponse> GenerateTokensAsync(ApplicationUser user)
        {
            var (jwtToken, jwtTokenId) = await CreateJwtToken(user);
            var refreshToken = await CreateAndSaveRefreshToken(jwtTokenId, user.Id);

            return new LoginResponse
            {
                Status = "Success",
                Message = "Login successful.",
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                RefreshToken = refreshToken,
                Expiration = jwtToken.ValidTo
            };
        }

        public async Task<LoginResponse> VerifyAndGenerateTokensAsync(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                // 1. Validate the expired token's signature and algorithm
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"])),
                    ValidateIssuer = _configuration.GetValue<bool>("JWT:ValidateIssuer"),
                    ValidateAudience = _configuration.GetValue<bool>("JWT:ValidateAudience"),
                    ValidateLifetime = false,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = jwtTokenHandler.ValidateToken(tokenRequest.AccessToken, tokenValidationParameters, out var validatedToken);

                // 2. Check the algorithm
                if (validatedToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return new LoginResponse { Status = "Error", Message = "Invalid token." };
                }

                // 3. Check if the token has expired
                var expiryDateUnix = long.Parse(principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);

                if (expiryDateTimeUtc > DateTime.UtcNow)
                {
                    return new LoginResponse { Status = "Error", Message = "Access token has not expired yet." };
                }

                // 4. Find the stored refresh token
                var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

                if (storedToken == null)
                {
                    return new LoginResponse { Status = "Error", Message = "Refresh token does not exist." };
                }

                // 5. Check if the refresh token is used or revoked
                if (storedToken.IsUsed || storedToken.IsRevoked)
                {
                    return new LoginResponse { Status = "Error", Message = "Refresh token has been used or revoked." };
                }

                // 6. Check if the refresh token has expired
                if (storedToken.ExpiryDate < DateTime.UtcNow)
                {
                    return new LoginResponse { Status = "Error", Message = "Refresh token has expired." };
                }

                // 7. Check if the JWT ID matches
                var jti = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    return new LoginResponse { Status = "Error", Message = "Token IDs do not match." };
                }

                // 8. Mark the current refresh token as used
                storedToken.IsUsed = true;
                _context.RefreshTokens.Update(storedToken);
                await _context.SaveChangesAsync();

                // 9. Generate new tokens
                var user = await _userManager.FindByIdAsync(storedToken.UserId);
                return await GenerateTokensAsync(user);
            }
            catch (Exception)
            {
                return new LoginResponse { Status = "Error", Message = "An error occurred." };
            }
        }


    }
}
