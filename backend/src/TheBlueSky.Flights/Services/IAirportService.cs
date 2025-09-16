using TheBlueSky.Flights.DTOs.Requests.Airport;
using TheBlueSky.Flights.DTOs.Responses.Airport;

namespace TheBlueSky.Flights.Services
{
    public interface IAirportService
    {
        Task<IEnumerable<AirportResponse>> GetAllAirportsAsync();
        Task<AirportResponse?> GetAirportByIdAsync(int id);
        Task<AirportResponse> CreateAirportAsync(CreateAirportRequest request);
        Task<bool> UpdateAirportAsync(UpdateAirportRequest request);
        Task<bool> DeleteAirportAsync(int id);

    }
}
