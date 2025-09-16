using TheBlueSky.Bookings.Models;

namespace TheBlueSky.Bookings.Repositories.Interfaces
{
    public interface IMealPreferenceRepository
    {
        Task<IEnumerable<MealPreference>> GetAllAsync();
        Task<MealPreference?> GetByIdAsync(int mealPreferenceId);
        Task<MealPreference> AddAsync(MealPreference mealPreference);
        Task<bool> UpdateAsync(MealPreference mealPreference);
        Task<bool> DeleteAsync(int mealPreferenceId);
        Task<bool> ExistsAsync(int mealPreferenceId);

    }
}
