using TheBlueSky.Flights.DTOs.Requests.AircraftSeat;
using TheBlueSky.Flights.DTOs.Responses.AircraftSeat;

namespace TheBlueSky.Flights.Services
{
    public interface IAircraftSeatService
    {
        Task<IEnumerable<AircraftSeatResponse>> GetAllAircraftSeatsAsync();
        Task<AircraftSeatResponse?> GetAircraftSeatByIdAsync(int id);
        Task<AircraftSeatResponse> CreateAircraftSeatAsync(CreateAircraftSeatRequest request);
        Task<bool> UpdateAircraftSeatAsync(UpdateAircraftSeatRequest request);
        Task<bool> DeleteAircraftSeatAsync(int id);

    }
}
