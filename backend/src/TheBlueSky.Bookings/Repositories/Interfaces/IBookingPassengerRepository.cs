using TheBlueSky.Bookings.Models;

namespace TheBlueSky.Bookings.Repositories.Interfaces
{
    public interface IBookingPassengerRepository
    {
        Task<IEnumerable<BookingPassenger>> GetAllAsync();
        Task<BookingPassenger?> GetByIdAsync(int bookingPassengerId);
        Task<BookingPassenger> AddAsync(BookingPassenger bookingPassenger);
        Task<bool> UpdateAsync(BookingPassenger bookingPassenger);
        Task<bool> DeleteAsync(int bookingPassengerId);
        Task<bool> ExistsAsync(int bookingPassengerId);

    }
}
