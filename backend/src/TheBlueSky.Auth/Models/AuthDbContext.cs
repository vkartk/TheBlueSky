using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TheBlueSky.Auth.Models
{
    public class AuthDbContext: IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options): base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }

}
