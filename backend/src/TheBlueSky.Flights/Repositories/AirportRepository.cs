using Microsoft.EntityFrameworkCore;
using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Repositories
{
    public class AirportRepository : IAirportRepository
    {
        private readonly FlightsDbContext _context;

        public AirportRepository(FlightsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Airport>> GetAllAirportsAsync()
        {
            return await _context.Airports.ToListAsync();
        }

        public async Task<Airport?> GetAirportByIdAsync(int id)
        {
            return await _context.Airports
                                 .Include(a => a.Country)
                                 .FirstOrDefaultAsync(a => a.AirportId == id);
        }

        public async Task AddAirportAsync(Airport airport)
        {
            _context.Airports.Add(airport);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAirportAsync(Airport airport)
        {
            _context.Entry(airport).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAirportAsync(int id)
        {
            var airport = await _context.Airports.FindAsync(id);
            if (airport != null)
            {
                _context.Airports.Remove(airport);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> AirportExists(int id)
        {
            return await _context.Airports.AnyAsync(e => e.AirportId == id);
        }
    }
}
