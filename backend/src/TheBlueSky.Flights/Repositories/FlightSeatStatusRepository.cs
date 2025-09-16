using Microsoft.EntityFrameworkCore;
using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Repositories
{
    public class FlightSeatStatusRepository : IFlightSeatStatusRepository
    {
        private readonly FlightsDbContext _context;

        public FlightSeatStatusRepository(FlightsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FlightSeatStatus>> GetAllFlightSeatStatusesAsync()
        {
            return await _context.FlightSeatStatuses.ToListAsync();
        }

        public async Task<FlightSeatStatus?> GetFlightSeatStatusByIdAsync(int id)
        {
            return await _context.FlightSeatStatuses.FindAsync(id);
        }

        public async Task<FlightSeatStatus> AddFlightSeatStatusAsync(FlightSeatStatus flightSeatStatus)
        {
            _context.FlightSeatStatuses.Add(flightSeatStatus);
            await _context.SaveChangesAsync();
            return flightSeatStatus;
        }

        public async Task<bool> UpdateFlightSeatStatusAsync(FlightSeatStatus flightSeatStatus)
        {
            _context.Entry(flightSeatStatus).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(flightSeatStatus.FlightSeatStatusId))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteFlightSeatStatusAsync(int id)
        {
            var flightSeatStatus = await _context.FlightSeatStatuses.FindAsync(id);
            if (flightSeatStatus == null)
            {
                return false;
            }

            _context.FlightSeatStatuses.Remove(flightSeatStatus);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.FlightSeatStatuses.AnyAsync(e => e.FlightSeatStatusId == id);
        }

    }
}
