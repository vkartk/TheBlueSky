using TheBlueSky.Bookings.Models;

namespace TheBlueSky.Bookings.Repositories.Interfaces
{
    public interface IBookingCancellationRepository
    {
        Task<IEnumerable<BookingCancellation>> GetAllAsync();
        Task<BookingCancellation?> GetByIdAsync(int bookingCancellationId);
        Task<BookingCancellation> AddAsync(BookingCancellation bookingCancellation);
        Task<bool> UpdateAsync(BookingCancellation bookingCancellation);
        Task<bool> DeleteAsync(int bookingCancellationId);
        Task<bool> ExistsAsync(int bookingCancellationId);

    }
}
