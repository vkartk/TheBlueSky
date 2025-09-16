using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.FlightSchedule;
using TheBlueSky.Flights.DTOs.Responses.FlightSchedule;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories;

namespace TheBlueSky.Flights.Services
{
    public class FlightScheduleService : IFlightScheduleService
    {
        private readonly IFlightScheduleRepository _flightScheduleRepository;
        private readonly IMapper _mapper;

        public FlightScheduleService(IFlightScheduleRepository flightScheduleRepository, IMapper mapper)
        {
            _flightScheduleRepository = flightScheduleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FlightScheduleResponse>> GetAllFlightSchedulesAsync()
        {
            var schedules = await _flightScheduleRepository.GetAllFlightSchedulesAsync();
            return _mapper.Map<IEnumerable<FlightScheduleResponse>>(schedules);
        }

        public async Task<FlightScheduleResponse?> GetFlightScheduleByIdAsync(int id)
        {
            var schedule = await _flightScheduleRepository.GetFlightScheduleByIdAsync(id);
            return _mapper.Map<FlightScheduleResponse>(schedule);
        }

        public async Task<FlightScheduleResponse> CreateFlightScheduleAsync(CreateFlightScheduleRequest request)
        {
            var schedule = _mapper.Map<FlightSchedule>(request);
            var createdSchedule = await _flightScheduleRepository.AddFlightScheduleAsync(schedule);
            return _mapper.Map<FlightScheduleResponse>(createdSchedule);
        }

        public async Task<bool> UpdateFlightScheduleAsync(UpdateFlightScheduleRequest request)
        {
            var existingSchedule = await _flightScheduleRepository.GetFlightScheduleByIdAsync(request.FlightScheduleId);
            if (existingSchedule == null)
            {
                return false;
            }

            _mapper.Map(request, existingSchedule);
            return await _flightScheduleRepository.UpdateFlightScheduleAsync(existingSchedule);
        }

        public async Task<bool> DeleteFlightScheduleAsync(int id)
        {
            return await _flightScheduleRepository.DeleteFlightScheduleAsync(id);
        }

    }
}
