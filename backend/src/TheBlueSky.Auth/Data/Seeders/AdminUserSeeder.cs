using Microsoft.AspNetCore.Identity;
using TheBlueSky.Auth.Models;

namespace TheBlueSky.Auth.Data.Seeders
{
    public class AdminUserSeeder : IDataSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AdminUserSeeder(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public int Priority => 2;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            var adminEmail = _configuration["DefaultAdminUser:Email"];
            if (string.IsNullOrEmpty(adminEmail)) return;

            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = _configuration["DefaultAdminUser:Username"],
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var password = _configuration["DefaultAdminUser:Password"];
                var result = await _userManager.CreateAsync(newAdmin, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newAdmin, UserRoles.Admin);
                }
            }
        }
    }
}
