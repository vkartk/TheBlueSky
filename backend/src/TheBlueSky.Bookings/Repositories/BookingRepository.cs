using Microsoft.EntityFrameworkCore;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories.Interfaces;

namespace TheBlueSky.Bookings.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingsDbContext _context;
        public BookingRepository(BookingsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings.AsNoTracking().OrderByDescending(b => b.BookingDate).ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == id);
        }
        public async Task<Booking> AddAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<bool> UpdateAsync(Booking booking)
        {
            _context.Entry(booking).State = EntityState.Modified;
            booking.LastUpdated = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(booking.BookingId)) return false;
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Bookings.FindAsync(id);

            if (existing is null) return false;

            _context.Bookings.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> ExistsAsync(int id)
        {
            return _context.Bookings.AnyAsync(b => b.BookingId == id);
        }
    }
}
