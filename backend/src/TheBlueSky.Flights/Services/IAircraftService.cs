using TheBlueSky.Flights.DTOs.Requests.Aircraft;
using TheBlueSky.Flights.DTOs.Responses.Aircraft;

namespace TheBlueSky.Flights.Services
{
    public interface IAircraftService
    {
        Task<IEnumerable<AircraftResponse>> GetAllAircraftsAsync();
        Task<AircraftResponse?> GetAircraftByIdAsync(int id);
        Task<AircraftResponse> CreateAircraftAsync(CreateAircraftRequest request);
        Task<bool> UpdateAircraftAsync(UpdateAircraftRequest request);
        Task<bool> DeleteAircraftAsync(int id);

    }
}
