using Microsoft.EntityFrameworkCore;
using TheBlueSky.Flights.Models;
using Route = TheBlueSky.Flights.Models.Route;

namespace TheBlueSky.Flights.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        private readonly FlightsDbContext _context;

        public RouteRepository(FlightsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Route>> GetAllRoutesAsync()
        {
            return await _context.Routes
                                 .Include(r => r.OriginAirport)
                                 .Include(r => r.DestinationAirport)
                                 .ToListAsync();
        }

        public async Task<Route?> GetRouteByIdAsync(int id)
        {
            return await _context.Routes
                                 .Include(r => r.OriginAirport)
                                 .Include(r => r.DestinationAirport)
                                 .FirstOrDefaultAsync(r => r.RouteId == id);
        }

        public async Task AddRouteAsync(Route route)
        {
            _context.Routes.Add(route);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRouteAsync(Route route)
        {
            _context.Entry(route).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRouteAsync(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route != null)
            {
                _context.Routes.Remove(route);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> RouteExists(int id)
        {
            return await _context.Routes.AnyAsync(e => e.RouteId == id);
        }

    }
}
