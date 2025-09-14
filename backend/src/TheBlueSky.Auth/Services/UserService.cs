using Microsoft.AspNetCore.Identity;
using TheBlueSky.Auth.Models;
using TheBlueSky.Auth.DTOs.Requests;

namespace TheBlueSky.Auth.Services
{
    public class UserService: IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegisterUser(CreateUserRequest createUserRequest)
        {
            var user = await _userManager.FindByEmailAsync(createUserRequest.Email);

            if (user != null)
            {
                return IdentityResult.Failed();
            }

            ApplicationUser applicationUser = new ApplicationUser()
            { 
                Email = createUserRequest.Email,
                UserName = createUserRequest.Email,
                SecurityStamp= Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(applicationUser);

            return result;

        }

    }
}
