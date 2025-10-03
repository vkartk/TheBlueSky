using TheBlueSky.Auth.DTOs.Requests;
using TheBlueSky.Auth.DTOs.Responses;
using TheBlueSky.Auth.Models;

namespace TheBlueSky.Auth.Services
{
    public interface IAuthTokenService
    {

        Task<LoginResponse> GenerateTokensAsync(ApplicationUser user);
        Task<LoginResponse> VerifyAndGenerateTokensAsync(TokenRequest tokenRequest);
    }
}
