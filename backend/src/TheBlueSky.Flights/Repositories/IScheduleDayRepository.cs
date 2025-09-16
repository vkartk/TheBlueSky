using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Repositories
{
    public interface IScheduleDayRepository
    {
        Task<IEnumerable<ScheduleDay>> GetAllScheduleDaysAsync();
        Task<ScheduleDay?> GetScheduleDayByIdAsync(int id);
        Task<ScheduleDay> AddScheduleDayAsync(ScheduleDay scheduleDay);
        Task<bool> UpdateScheduleDayAsync(ScheduleDay scheduleDay);
        Task<bool> DeleteScheduleDayAsync(int id);
        Task<bool> ExistsAsync(int id);

    }
}
