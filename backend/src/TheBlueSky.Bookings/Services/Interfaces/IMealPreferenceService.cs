using TheBlueSky.Bookings.DTOs.Requests.MealPreference;
using TheBlueSky.Bookings.DTOs.Responses.MealPreference;

namespace TheBlueSky.Bookings.Services.Interfaces
{
    public interface IMealPreferenceService
    {
        Task<IEnumerable<MealPreferenceResponse>> GetAllAsync();
        Task<MealPreferenceResponse?> GetByIdAsync(int mealPreferenceId);
        Task<MealPreferenceResponse> CreateAsync(CreateMealPreferenceRequest request);
        Task<bool> UpdateAsync(UpdateMealPreferenceRequest request);
        Task<bool> DeleteAsync(int mealPreferenceId);
    }
}
