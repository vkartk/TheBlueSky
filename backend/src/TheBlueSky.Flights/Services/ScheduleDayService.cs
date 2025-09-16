using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.ScheduleDay;
using TheBlueSky.Flights.DTOs.Responses.ScheduleDay;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories;

namespace TheBlueSky.Flights.Services
{
    public class ScheduleDayService : IScheduleDayService
    {
        private readonly IScheduleDayRepository _scheduleDayRepository;
        private readonly IMapper _mapper;

        public ScheduleDayService(IScheduleDayRepository scheduleDayRepository, IMapper mapper)
        {
            _scheduleDayRepository = scheduleDayRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ScheduleDayResponse>> GetAllScheduleDaysAsync()
        {
            var days = await _scheduleDayRepository.GetAllScheduleDaysAsync();
            return _mapper.Map<IEnumerable<ScheduleDayResponse>>(days);
        }

        public async Task<ScheduleDayResponse?> GetScheduleDayByIdAsync(int id)
        {
            var day = await _scheduleDayRepository.GetScheduleDayByIdAsync(id);
            return _mapper.Map<ScheduleDayResponse>(day);
        }

        public async Task<ScheduleDayResponse> CreateScheduleDayAsync(CreateScheduleDayRequest request)
        {
            var day = _mapper.Map<ScheduleDay>(request);
            var createdDay = await _scheduleDayRepository.AddScheduleDayAsync(day);
            return _mapper.Map<ScheduleDayResponse>(createdDay);
        }

        public async Task<bool> UpdateScheduleDayAsync(UpdateScheduleDayRequest request)
        {
            var existingDay = await _scheduleDayRepository.GetScheduleDayByIdAsync(request.ScheduleDayId);
            if (existingDay == null)
            {
                return false;
            }

            _mapper.Map(request, existingDay);
            return await _scheduleDayRepository.UpdateScheduleDayAsync(existingDay);
        }

        public async Task<bool> DeleteScheduleDayAsync(int id)
        {
            return await _scheduleDayRepository.DeleteScheduleDayAsync(id);
        }

    }
}
