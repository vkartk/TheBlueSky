using Microsoft.EntityFrameworkCore;

namespace TheBlueSky.Flights.Models
{
    public class FlightsDbContext : DbContext
    {
        public FlightsDbContext(DbContextOptions<FlightsDbContext> options) : base(options) { }
    }
}
