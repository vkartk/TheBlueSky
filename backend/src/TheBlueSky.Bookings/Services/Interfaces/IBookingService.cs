using TheBlueSky.Bookings.DTOs.Requests.Booking;
using TheBlueSky.Bookings.DTOs.Responses.Booking;

namespace TheBlueSky.Bookings.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingResponse>> GetAllAsync();
        Task<BookingResponse?> GetByIdAsync(int id);
        Task<BookingResponse> CreateAsync(CreateBookingRequest request);
        Task<bool> UpdateAsync(UpdateBookingRequest request);
        Task<bool> DeleteAsync(int id);

    }
}
