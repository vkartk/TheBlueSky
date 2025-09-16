using AutoMapper;
using TheBlueSky.Bookings.DTOs.Requests.MealPreference;
using TheBlueSky.Bookings.DTOs.Responses.MealPreference;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories.Interfaces;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Services
{
    public class MealPreferenceService : IMealPreferenceService
    {
        private readonly IMealPreferenceRepository _repository;
        private readonly IMapper _mapper;

        public MealPreferenceService(IMealPreferenceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MealPreferenceResponse>> GetAllAsync()
        {
            var mealPreferences = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<MealPreferenceResponse>>(mealPreferences);
        }

        public async Task<MealPreferenceResponse?> GetByIdAsync(int mealPreferenceId)
        {
            var mealPreference = await _repository.GetByIdAsync(mealPreferenceId);
            return mealPreference is null ? null : _mapper.Map<MealPreferenceResponse>(mealPreference);
        }

        public async Task<MealPreferenceResponse> CreateAsync(CreateMealPreferenceRequest request)
        {
            var mealPreference = _mapper.Map<MealPreference>(request);
            var created = await _repository.AddAsync(mealPreference);
            return _mapper.Map<MealPreferenceResponse>(created);
        }

        public async Task<bool> UpdateAsync(UpdateMealPreferenceRequest request)
        {
            var mealPreference = _mapper.Map<MealPreference>(request);
            return await _repository.UpdateAsync(mealPreference);
        }

        public Task<bool> DeleteAsync(int mealPreferenceId)
        {
            return _repository.DeleteAsync(mealPreferenceId);
        }

    }
}
