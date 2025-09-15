using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Services
{
    public interface IAirportService
    {
        Task<IEnumerable<Airport>> GetAllAirportsAsync();
        Task<Airport?> GetAirportByIdAsync(int id);
        Task<Airport> CreateAirportAsync(Airport airport);
        Task<bool> UpdateAirportAsync(int id, Airport airport);
        Task<bool> DeleteAirportAsync(int id);

    }
}
