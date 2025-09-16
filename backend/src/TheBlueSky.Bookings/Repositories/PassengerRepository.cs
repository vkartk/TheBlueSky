using Microsoft.EntityFrameworkCore;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories.Interfaces;

namespace TheBlueSky.Bookings.Repositories
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly BookingsDbContext _context;

        public PassengerRepository(BookingsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Passenger>> GetAllAsync()
        {
            return await _context.Passengers
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }

        public async Task<Passenger?> GetByIdAsync(int passengerId)
        {
            return await _context.Passengers
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PassengerId == passengerId);
        }

        public async Task<Passenger> AddAsync(Passenger passenger)
        {
            _context.Passengers.Add(passenger);
            await _context.SaveChangesAsync();
            return passenger;
        }

        public async Task<bool> UpdateAsync(Passenger passenger)
        {
            _context.Entry(passenger).State = EntityState.Modified;
            _context.Entry(passenger).Property(p => p.CreatedDate).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(passenger.PassengerId))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int passengerId)
        {
            var passenger = await _context.Passengers.FindAsync(passengerId);
            if (passenger == null) return false;

            _context.Passengers.Remove(passenger);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(passengerId))
                {
                    return false;
                }
                throw;
            }
        }

        public Task<bool> ExistsAsync(int passengerId)
        {
            return _context.Passengers.AnyAsync(p => p.PassengerId == passengerId);
        }
    }
}
