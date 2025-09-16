using Microsoft.EntityFrameworkCore;
using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Repositories
{
    public class AircraftRepository : IAircraftRepository
    {
        private readonly FlightsDbContext _context;

        public AircraftRepository(FlightsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Aircraft>> GetAllAircraftsAsync()
        {
            return await _context.Aircrafts.ToListAsync();
        }

        public async Task<Aircraft?> GetAircraftByIdAsync(int id)
        {
            return await _context.Aircrafts.FindAsync(id);
        }

        public async Task<Aircraft> AddAircraftAsync(Aircraft aircraft)
        {
            _context.Aircrafts.Add(aircraft);
            await _context.SaveChangesAsync();
            return aircraft;
        }

        public async Task<bool> UpdateAircraftAsync(Aircraft aircraft)
        {
            _context.Entry(aircraft).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(aircraft.AircraftId))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteAircraftAsync(int id)
        {
            var aircraft = await _context.Aircrafts.FindAsync(id);
            if (aircraft == null)
            {
                return false;
            }

            _context.Aircrafts.Remove(aircraft);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Aircrafts.AnyAsync(e => e.AircraftId == id);
        }

    }
}
