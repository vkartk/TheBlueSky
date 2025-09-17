using Microsoft.EntityFrameworkCore;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories.Interfaces;

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

        public async Task<Airport> AddAirportAsync(Airport airport)
        {
            _context.Airports.Add(airport);
            await _context.SaveChangesAsync();
            return airport;
        }

        public async Task<bool> UpdateAirportAsync(Airport airport)
        {
            _context.Entry(airport).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(airport.AirportId))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteAirportAsync(int id)
        {
            var airport = await _context.Airports.FindAsync(id);
            if (airport == null)
            {
                return false;
            }

            _context.Airports.Remove(airport);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Airports.AnyAsync(e => e.AirportId == id);
        }

    }
}
