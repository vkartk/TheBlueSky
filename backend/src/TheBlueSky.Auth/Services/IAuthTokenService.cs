using System.IdentityModel.Tokens.Jwt;
using TheBlueSky.Auth.Models;

namespace TheBlueSky.Auth.Services
{
    public interface IAuthTokenService
    {
        Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user);
    }
}
