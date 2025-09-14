using Microsoft.AspNetCore.Identity;
using TheBlueSky.Auth.DTOs.Requests;

namespace TheBlueSky.Auth.Services
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUser(CreateUserRequest createUserRequest);
    }
}
