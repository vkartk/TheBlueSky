using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Repositories
{
    public interface IAircraftRepository
    {
        Task<IEnumerable<Aircraft>> GetAllAircraftsAsync();
        Task<Aircraft?> GetAircraftByIdAsync(int id);
        Task<Aircraft> AddAircraftAsync(Aircraft aircraft);
        Task<bool> UpdateAircraftAsync(Aircraft aircraft);
        Task<bool> DeleteAircraftAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
