using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TheBlueSky.Auth.DTOs.Requests;
using TheBlueSky.Auth.DTOs.Responses;
using TheBlueSky.Auth.Models;
using TheBlueSky.Auth.Services;

namespace TheBlueSky.Auth.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly IAuthTokenService _authTokenService;
        private readonly AuthDbContext _context;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            IAuthTokenService authTokenService,
            AuthDbContext context)
        {
            _userManager = userManager;
            _userService = userService;
            _authTokenService = authTokenService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new AuthResponse { Status = "Error", Message = "Invalid request." });

            try
            {
                var result = await _userService.RegisterUser(request);

                if (!result.Succeeded)
                    return BadRequest(new AuthResponse { Status = "Error", Message = "Registration failed. Please try again." });

                var createdUser = await _userManager.FindByEmailAsync(request.Email);

                if (createdUser == null)
                    return StatusCode(500, new AuthResponse { Status = "Error", Message = "Something went wrong. Please try again." });

                if (!await _userManager.IsInRoleAsync(createdUser, UserRoles.User))
                    await _userManager.AddToRoleAsync(createdUser, UserRoles.User);

                return Ok(new AuthResponse { Status = "Success", Message = "User created successfully!" });
            }
            catch
            {
                return StatusCode(500, new AuthResponse { Status = "Error", Message = "An error occurred. Please try again." });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Status = "Error", Message = "Invalid request." });

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var tokenResponse = await _authTokenService.GenerateTokensAsync(user);
                return Ok(tokenResponse);
            }

            return Unauthorized(new { Status = "Error", Message = "Invalid credentials." });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Status = "Error", Message = "Invalid request." });

            var result = await _authTokenService.VerifyAndGenerateTokensAsync(tokenRequest);

            if (result.Status == "Error")
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest logoutRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Status = "Error", Message = "Invalid request." });

            // Find and revoke the refresh token
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == logoutRequest.RefreshToken);

            if (refreshToken == null)
                return BadRequest(new { Status = "Error", Message = "Invalid refresh token." });

            // Ensure the token belongs to the currently logged-in user
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (refreshToken.UserId != userId)
                return Unauthorized();

            refreshToken.IsRevoked = true;
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();

            return Ok(new { Status = "Success", Message = "User logged out successfully." });
        }

        [HttpPost("admin/register")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> RegisterAdmin([FromBody] CreateUserRequest request)
        {
            try
            {
                var result = await _userService.RegisterUser(request);

                if (!result.Succeeded)
                    return BadRequest(new AuthResponse { Status = "Error", Message = "Registration failed. Please try again." });

                var createdUser = await _userManager.FindByEmailAsync(request.Email);

                if (createdUser == null)
                    return StatusCode(500, new AuthResponse { Status = "Error", Message = "Something went wrong. Please try again." });

                await _userManager.AddToRoleAsync(createdUser, UserRoles.Admin);

                return Ok(new AuthResponse { Status = "Success", Message = "Admin created successfully!" });
            }
            catch
            {
                return StatusCode(500, new AuthResponse { Status = "Error", Message = "An error occurred. Please try again." });
            }
        }

        [HttpGet("whoami")]
        [Authorize]
        public  IActionResult WhoAmI()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var email = User.FindFirstValue(ClaimTypes.Email);
                var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

                return Ok(new
                {
                    userId,
                    email,
                    roles
                });
            }
            catch
            {
                return StatusCode(500, new AuthResponse { Status = "Error", Message = "An error occurred. Please try again." });
            }
        }
    }
}