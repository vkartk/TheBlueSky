using Microsoft.EntityFrameworkCore;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories.Interfaces;

namespace TheBlueSky.Bookings.Repositories
{
    public class BookingCancellationRepository : IBookingCancellationRepository
    {
        private readonly BookingsDbContext _context;

        public BookingCancellationRepository(BookingsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookingCancellation>> GetAllAsync()
        {
            return await _context.BookingCancellations
                .AsNoTracking()
                .OrderByDescending(bc => bc.CancellationDate)
                .ToListAsync();
        }

        public async Task<BookingCancellation?> GetByIdAsync(int bookingCancellationId)
        {
            return await _context.BookingCancellations
                .AsNoTracking()
                .FirstOrDefaultAsync(bc => bc.BookingCancellationId == bookingCancellationId);
        }

        public async Task<BookingCancellation> AddAsync(BookingCancellation bookingCancellation)
        {
            _context.BookingCancellations.Add(bookingCancellation);
            await _context.SaveChangesAsync();
            return bookingCancellation;
        }

        public async Task<bool> UpdateAsync(BookingCancellation bookingCancellation)
        {
            _context.Entry(bookingCancellation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(bookingCancellation.BookingCancellationId))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int bookingCancellationId)
        {
            var bookingCancellation = await _context.BookingCancellations.FindAsync(bookingCancellationId);
            if (bookingCancellation == null) return false;

            _context.BookingCancellations.Remove(bookingCancellation);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(bookingCancellationId))
                {
                    return false;
                }
                throw;
            }
        }

        public Task<bool> ExistsAsync(int bookingCancellationId)
        {
            return _context.BookingCancellations.AnyAsync(bc => bc.BookingCancellationId == bookingCancellationId);
        }
    }
}
