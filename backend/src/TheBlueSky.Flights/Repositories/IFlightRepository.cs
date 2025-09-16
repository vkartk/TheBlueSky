using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Repositories
{
    public interface IFlightRepository
    {
        Task<IEnumerable<Flight>> GetAllFlightsAsync();
        Task<Flight?> GetFlightByIdAsync(int id);
        Task<Flight> AddFlightAsync(Flight flight);
        Task<bool> UpdateFlightAsync(Flight flight);
        Task<bool> DeleteFlightAsync(int id);
        Task<bool> ExistsAsync(int id);

    }
}
