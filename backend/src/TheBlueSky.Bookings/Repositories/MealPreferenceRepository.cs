using Microsoft.EntityFrameworkCore;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories.Interfaces;

namespace TheBlueSky.Bookings.Repositories
{
    public class MealPreferenceRepository : IMealPreferenceRepository
    {
        private readonly BookingsDbContext _context;

        public MealPreferenceRepository(BookingsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MealPreference>> GetAllAsync()
        {
            return await _context.MealPreferences
                .AsNoTracking()
                .OrderByDescending(m => m.MealPreferenceId)
                .ToListAsync();
        }

        public async Task<MealPreference?> GetByIdAsync(int mealPreferenceId)
        {
            return await _context.MealPreferences
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MealPreferenceId == mealPreferenceId);
        }

        public async Task<MealPreference> AddAsync(MealPreference mealPreference)
        {
            _context.MealPreferences.Add(mealPreference);
            await _context.SaveChangesAsync();
            return mealPreference;
        }

        public async Task<bool> UpdateAsync(MealPreference mealPreference)
        {
            _context.Entry(mealPreference).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(mealPreference.MealPreferenceId))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int mealPreferenceId)
        {
            var mealPreference = await _context.MealPreferences.FindAsync(mealPreferenceId);
            if (mealPreference == null) return false;

            _context.MealPreferences.Remove(mealPreference);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(mealPreferenceId))
                {
                    return false;
                }
                throw;
            }
        }

        public Task<bool> ExistsAsync(int mealPreferenceId)
        {
            return _context.MealPreferences.AnyAsync(m => m.MealPreferenceId == mealPreferenceId);
        }
    }
}
