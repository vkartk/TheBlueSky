using Microsoft.AspNetCore.Identity;
using TheBlueSky.Auth.Models;

namespace TheBlueSky.Auth.Data.Seeders
{
    public class RoleSeeder: IDataSeeder
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleSeeder(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public int Priority => 1;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            string[] roles = { UserRoles.Admin, UserRoles.User, UserRoles.FlightsOwner };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

        }
    }
}
