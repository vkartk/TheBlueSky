using TheBlueSky.Flights.DTOs.Requests.FlightSchedule;
using TheBlueSky.Flights.DTOs.Responses.FlightSchedule;

namespace TheBlueSky.Flights.Services
{
    public interface IFlightScheduleService
    {
        Task<IEnumerable<FlightScheduleResponse>> GetAllFlightSchedulesAsync();
        Task<FlightScheduleResponse?> GetFlightScheduleByIdAsync(int id);
        Task<FlightScheduleResponse> CreateFlightScheduleAsync(CreateFlightScheduleRequest request);
        Task<bool> UpdateFlightScheduleAsync(UpdateFlightScheduleRequest request);
        Task<bool> DeleteFlightScheduleAsync(int id);
    }

}
