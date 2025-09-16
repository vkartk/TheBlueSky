using TheBlueSky.Flights.DTOs.Requests.Flight;
using TheBlueSky.Flights.DTOs.Responses.Flight;

namespace TheBlueSky.Flights.Services
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightResponse>> GetAllFlightsAsync();
        Task<FlightResponse?> GetFlightByIdAsync(int id);
        Task<FlightResponse> CreateFlightAsync(CreateFlightRequest request);
        Task<bool> UpdateFlightAsync(UpdateFlightRequest request);
        Task<bool> DeleteFlightAsync(int id);

    }
}
