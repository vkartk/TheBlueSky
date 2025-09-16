using TheBlueSky.Flights.DTOs.Requests.ScheduleDay;
using TheBlueSky.Flights.DTOs.Responses.ScheduleDay;

namespace TheBlueSky.Flights.Services
{
    public interface IScheduleDayService
    {
        Task<IEnumerable<ScheduleDayResponse>> GetAllScheduleDaysAsync();
        Task<ScheduleDayResponse?> GetScheduleDayByIdAsync(int id);
        Task<ScheduleDayResponse> CreateScheduleDayAsync(CreateScheduleDayRequest request);
        Task<bool> UpdateScheduleDayAsync(UpdateScheduleDayRequest request);
        Task<bool> DeleteScheduleDayAsync(int id);

    }
}
