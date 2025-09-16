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
            return await _context.Routes.ToListAsync();
        }

        public async Task<Route?> GetRouteByIdAsync(int id)
        {
            return await _context.Routes.FindAsync(id);
        }

        public async Task<Route> AddRouteAsync(Route route)
        {
            _context.Routes.Add(route);
            await _context.SaveChangesAsync();
            return route;
        }

        public async Task<bool> UpdateRouteAsync(Route route)
        {
            _context.Entry(route).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(route.RouteId))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteRouteAsync(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null)
            {
                return false;
            }

            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Routes.AnyAsync(e => e.RouteId == id);
        }
    }
}
