using Microsoft.EntityFrameworkCore;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories.Interfaces;

namespace TheBlueSky.Flights.Repositories
{
    public class AircraftSeatRepository : IAircraftSeatRepository
    {
        private readonly FlightsDbContext _context;

        public AircraftSeatRepository(FlightsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AircraftSeat>> GetAllAircraftSeatsAsync()
        {
            return await _context.AircraftSeats.ToListAsync();
        }

        public async Task<AircraftSeat?> GetAircraftSeatByIdAsync(int id)
        {
            return await _context.AircraftSeats.FindAsync(id);
        }

        public async Task<AircraftSeat> AddAircraftSeatAsync(AircraftSeat aircraftSeat)
        {
            _context.AircraftSeats.Add(aircraftSeat);
            await _context.SaveChangesAsync();
            return aircraftSeat;
        }

        public async Task<bool> UpdateAircraftSeatAsync(AircraftSeat aircraftSeat)
        {
            _context.Entry(aircraftSeat).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(aircraftSeat.AircraftSeatId))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteAircraftSeatAsync(int id)
        {
            var aircraftSeat = await _context.AircraftSeats.FindAsync(id);
            if (aircraftSeat == null)
            {
                return false;
            }

            _context.AircraftSeats.Remove(aircraftSeat);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.AircraftSeats.AnyAsync(e => e.AircraftSeatId == id);
        }

    }
}
