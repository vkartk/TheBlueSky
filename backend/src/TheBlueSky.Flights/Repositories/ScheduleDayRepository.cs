using Microsoft.EntityFrameworkCore;
using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Repositories
{
    public class ScheduleDayRepository : IScheduleDayRepository
    {
        private readonly FlightsDbContext _context;

        public ScheduleDayRepository(FlightsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ScheduleDay>> GetAllScheduleDaysAsync()
        {
            return await _context.ScheduleDays.ToListAsync();
        }

        public async Task<ScheduleDay?> GetScheduleDayByIdAsync(int id)
        {
            return await _context.ScheduleDays.FindAsync(id);
        }

        public async Task<ScheduleDay> AddScheduleDayAsync(ScheduleDay scheduleDay)
        {
            _context.ScheduleDays.Add(scheduleDay);
            await _context.SaveChangesAsync();
            return scheduleDay;
        }

        public async Task<bool> UpdateScheduleDayAsync(ScheduleDay scheduleDay)
        {
            _context.Entry(scheduleDay).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(scheduleDay.ScheduleDayId))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteScheduleDayAsync(int id)
        {
            var scheduleDay = await _context.ScheduleDays.FindAsync(id);
            if (scheduleDay == null)
            {
                return false;
            }

            _context.ScheduleDays.Remove(scheduleDay);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ScheduleDays.AnyAsync(e => e.ScheduleDayId == id);
        }
    }
}
