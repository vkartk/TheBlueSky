using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Repositories
{
    public interface IAirportRepository
    {
        Task<IEnumerable<Airport>> GetAllAirportsAsync();
        Task<Airport?> GetAirportByIdAsync(int id);
        Task AddAirportAsync(Airport airport);
        Task UpdateAirportAsync(Airport airport);
        Task DeleteAirportAsync(int id);
        Task<bool> AirportExists(int id);

    }
}
