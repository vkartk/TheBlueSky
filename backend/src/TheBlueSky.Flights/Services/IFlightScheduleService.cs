using TheBlueSky.Flights.DTOs.Requests.FlightSchedule;
using TheBlueSky.Flights.DTOs.Responses.Flight;
using TheBlueSky.Flights.DTOs.Responses.FlightSchedule;
using TheBlueSky.Flights.DTOs.Responses.ScheduleDay;

namespace TheBlueSky.Flights.Services
{
    public interface IFlightScheduleService
    {
        Task<IEnumerable<FlightScheduleResponse>> GetAllFlightSchedulesAsync();
        Task<FlightScheduleResponse?> GetFlightScheduleByIdAsync(int id);
        Task<FlightScheduleResponse> CreateFlightScheduleAsync(CreateFlightScheduleRequest request);
        Task<bool> UpdateFlightScheduleAsync(UpdateFlightScheduleRequest request);
        Task<bool> DeleteFlightScheduleAsync(int id);

        Task<IEnumerable<FlightResponse>> GetFlightsForScheduleAsync(int flightScheduleId);
        Task<IEnumerable<ScheduleDayResponse>> UpdateScheduleDaysAsync(int flightScheduleId, IEnumerable<string> daysOfWeek);
        Task<int> GenerateFlightsAsync(int flightScheduleId, DateOnly startDate, DateOnly endDate);

    }

}
