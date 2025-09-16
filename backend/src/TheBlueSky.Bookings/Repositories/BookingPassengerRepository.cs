using Microsoft.EntityFrameworkCore;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories.Interfaces;

namespace TheBlueSky.Bookings.Repositories
{
    public class BookingPassengerRepository : IBookingPassengerRepository
    {
        private readonly BookingsDbContext _context;

        public BookingPassengerRepository(BookingsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookingPassenger>> GetAllAsync()
        {
            return await _context.BookingPassengers
                .AsNoTracking()
                .OrderByDescending(bp => bp.BookingPassengerId)
                .ToListAsync();
        }

        public async Task<BookingPassenger?> GetByIdAsync(int bookingPassengerId)
        {
            return await _context.BookingPassengers
                .AsNoTracking()
                .FirstOrDefaultAsync(bp => bp.BookingPassengerId == bookingPassengerId);
        }

        public async Task<BookingPassenger> AddAsync(BookingPassenger bookingPassenger)
        {
            _context.BookingPassengers.Add(bookingPassenger);
            await _context.SaveChangesAsync();
            return bookingPassenger;
        }

        public async Task<bool> UpdateAsync(BookingPassenger bookingPassenger)
        {
            _context.Entry(bookingPassenger).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(bookingPassenger.BookingPassengerId))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int bookingPassengerId)
        {
            var bookingPassenger = await _context.BookingPassengers.FindAsync(bookingPassengerId);
            if (bookingPassenger == null) return false;

            _context.BookingPassengers.Remove(bookingPassenger);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(bookingPassengerId))
                {
                    return false;
                }
                throw;
            }
        }

        public Task<bool> ExistsAsync(int bookingPassengerId)
        {
            return _context.BookingPassengers.AnyAsync(bp => bp.BookingPassengerId == bookingPassengerId);
        }

    }
}
