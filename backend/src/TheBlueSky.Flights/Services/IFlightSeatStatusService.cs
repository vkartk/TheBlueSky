using TheBlueSky.Flights.DTOs.Requests.FlightSeatStatus;
using TheBlueSky.Flights.DTOs.Responses.FlightSeatStatus;

namespace TheBlueSky.Flights.Services
{
    public interface IFlightSeatStatusService
    {
        Task<IEnumerable<FlightSeatStatusResponse>> GetAllFlightSeatStatusesAsync();
        Task<FlightSeatStatusResponse?> GetFlightSeatStatusByIdAsync(int id);
        Task<FlightSeatStatusResponse> CreateFlightSeatStatusAsync(CreateFlightSeatStatusRequest request);
        Task<bool> UpdateFlightSeatStatusAsync(UpdateFlightSeatStatusRequest request);
        Task<bool> DeleteFlightSeatStatusAsync(int id);

    }
}
