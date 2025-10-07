using TheBlueSky.Bookings.DTOs.Requests.BookingPassenger;
using TheBlueSky.Bookings.DTOs.Responses.BookingPassenger;
using TheBlueSky.Bookings.Models;

namespace TheBlueSky.Bookings.Services.Interfaces
{
    public interface IBookingPassengerService
    {
        Task<IEnumerable<BookingPassengerResponse>> GetAllAsync();
        Task<BookingPassengerResponse?> GetByIdAsync(int bookingPassengerId);
        Task<BookingPassengerResponse> CreateAsync(CreateBookingPassengerRequest request);
        Task<bool> UpdateAsync(UpdateBookingPassengerRequest request);
        Task<bool> DeleteAsync(int bookingPassengerId);

    }
}
