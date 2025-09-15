using Microsoft.EntityFrameworkCore;
using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Repositories
{
    public class SeatClassRepository : ISeatClassRepository
    {
        private readonly FlightsDbContext _context;

        public SeatClassRepository(FlightsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SeatClass>> GetAllSeatClassesAsync()
        {
            return await _context.SeatClasses.ToListAsync();
        }

        public async Task<SeatClass?> GetSeatClassByIdAsync(int id)
        {
            return await _context.SeatClasses
                                 .Include(sc => sc.Seats)
                                 .FirstOrDefaultAsync(sc => sc.SeatClassId == id);
        }

        public async Task AddSeatClassAsync(SeatClass seatClass)
        {
            _context.SeatClasses.Add(seatClass);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSeatClassAsync(SeatClass seatClass)
        {
            _context.Entry(seatClass).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSeatClassAsync(int id)
        {
            var seatClass = await _context.SeatClasses.FindAsync(id);
            if (seatClass != null)
            {
                _context.SeatClasses.Remove(seatClass);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> SeatClassExists(int id)
        {
            return await _context.SeatClasses.AnyAsync(e => e.SeatClassId == id);
        }

    }
}
