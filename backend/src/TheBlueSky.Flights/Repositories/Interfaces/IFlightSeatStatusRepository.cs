using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Repositories.Interfaces
{
    public interface IFlightSeatStatusRepository
    {
        Task<IEnumerable<FlightSeatStatus>> GetAllFlightSeatStatusesAsync();
        Task<FlightSeatStatus?> GetFlightSeatStatusByIdAsync(int id);
        Task<FlightSeatStatus> AddFlightSeatStatusAsync(FlightSeatStatus flightSeatStatus);
        Task<bool> UpdateFlightSeatStatusAsync(FlightSeatStatus flightSeatStatus);
        Task<bool> DeleteFlightSeatStatusAsync(int id);
        Task<bool> ExistsAsync(int id);

    }
}
