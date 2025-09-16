using Microsoft.EntityFrameworkCore;
using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Repositories
{
    public class FlightScheduleRepository : IFlightScheduleRepository
    {
        private readonly FlightsDbContext _context;

        public FlightScheduleRepository(FlightsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FlightSchedule>> GetAllFlightSchedulesAsync()
        {
            return await _context.FlightSchedules.ToListAsync();
        }

        public async Task<FlightSchedule?> GetFlightScheduleByIdAsync(int id)
        {
            return await _context.FlightSchedules.FindAsync(id);
        }

        public async Task<FlightSchedule> AddFlightScheduleAsync(FlightSchedule flightSchedule)
        {
            _context.FlightSchedules.Add(flightSchedule);
            await _context.SaveChangesAsync();
            return flightSchedule;
        }

        public async Task<bool> UpdateFlightScheduleAsync(FlightSchedule flightSchedule)
        {
            _context.Entry(flightSchedule).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(flightSchedule.FlightScheduleId))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteFlightScheduleAsync(int id)
        {
            var flightSchedule = await _context.FlightSchedules.FindAsync(id);
            if (flightSchedule == null)
            {
                return false;
            }

            _context.FlightSchedules.Remove(flightSchedule);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.FlightSchedules.AnyAsync(e => e.FlightScheduleId == id);
        }

    }
}
