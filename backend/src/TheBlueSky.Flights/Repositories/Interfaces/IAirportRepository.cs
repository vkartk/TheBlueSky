using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Repositories.Interfaces
{
    public interface IAirportRepository
    {
        Task<IEnumerable<Airport>> GetAllAirportsAsync();
        Task<Airport?> GetAirportByIdAsync(int id);
        Task<Airport> AddAirportAsync(Airport airport);
        Task<bool> UpdateAirportAsync(Airport airport);
        Task<bool> DeleteAirportAsync(int id);
        Task<bool> ExistsAsync(int id);


    }
}
