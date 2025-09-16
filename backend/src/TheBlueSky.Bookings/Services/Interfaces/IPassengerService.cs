using TheBlueSky.Bookings.DTOs.Requests.Passenger;
using TheBlueSky.Bookings.DTOs.Responses.Passenger;

namespace TheBlueSky.Bookings.Services.Interfaces
{
    public interface IPassengerService
    {
        Task<IEnumerable<PassengerResponse>> GetAllAsync();
        Task<PassengerResponse?> GetByIdAsync(int passengerId);
        Task<PassengerResponse> CreateAsync(CreatePassengerRequest request);
        Task<bool> UpdateAsync(UpdatePassengerRequest request);
        Task<bool> DeleteAsync(int passengerId);

    }
}
