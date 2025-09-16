using TheBlueSky.Bookings.DTOs.Requests.BookingCancellation;
using TheBlueSky.Bookings.DTOs.Responses.BookingCancellation;

namespace TheBlueSky.Bookings.Services.Interfaces
{
    public interface IBookingCancellationService
    {
        Task<IEnumerable<BookingCancellationResponse>> GetAllAsync();
        Task<BookingCancellationResponse?> GetByIdAsync(int bookingCancellationId);
        Task<BookingCancellationResponse> CreateAsync(CreateBookingCancellationRequest request);
        Task<bool> UpdateAsync(UpdateBookingCancellationRequest request);
        Task<bool> DeleteAsync(int bookingCancellationId);

    }
}
