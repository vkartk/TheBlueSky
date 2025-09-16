using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Repositories
{
    public interface IFlightScheduleRepository
    {
        Task<IEnumerable<FlightSchedule>> GetAllFlightSchedulesAsync();
        Task<FlightSchedule?> GetFlightScheduleByIdAsync(int id);
        Task<FlightSchedule> AddFlightScheduleAsync(FlightSchedule flightSchedule);
        Task<bool> UpdateFlightScheduleAsync(FlightSchedule flightSchedule);
        Task<bool> DeleteFlightScheduleAsync(int id);
        Task<bool> ExistsAsync(int id);

    }
}
