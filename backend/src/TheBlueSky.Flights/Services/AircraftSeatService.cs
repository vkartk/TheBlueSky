using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.AircraftSeat;
using TheBlueSky.Flights.DTOs.Responses.AircraftSeat;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories;

namespace TheBlueSky.Flights.Services
{
    public class AircraftSeatService : IAircraftSeatService
    {
        private readonly IAircraftSeatRepository _aircraftSeatRepository;
        private readonly IMapper _mapper;

        public AircraftSeatService(IAircraftSeatRepository aircraftSeatRepository, IMapper mapper)
        {
            _aircraftSeatRepository = aircraftSeatRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AircraftSeatResponse>> GetAllAircraftSeatsAsync()
        {
            var aircraftSeats = await _aircraftSeatRepository.GetAllAircraftSeatsAsync();
            return _mapper.Map<IEnumerable<AircraftSeatResponse>>(aircraftSeats);
        }

        public async Task<AircraftSeatResponse?> GetAircraftSeatByIdAsync(int id)
        {
            var aircraftSeat = await _aircraftSeatRepository.GetAircraftSeatByIdAsync(id);
            return _mapper.Map<AircraftSeatResponse>(aircraftSeat);
        }

        public async Task<AircraftSeatResponse> CreateAircraftSeatAsync(CreateAircraftSeatRequest request)
        {
            var aircraftSeat = _mapper.Map<AircraftSeat>(request);
            var createdSeat = await _aircraftSeatRepository.AddAircraftSeatAsync(aircraftSeat);
            return _mapper.Map<AircraftSeatResponse>(createdSeat);
        }

        public async Task<bool> UpdateAircraftSeatAsync(UpdateAircraftSeatRequest request)
        {
            var existingSeat = await _aircraftSeatRepository.GetAircraftSeatByIdAsync(request.AircraftSeatId);
            if (existingSeat == null)
            {
                return false;
            }

            _mapper.Map(request, existingSeat);
            return await _aircraftSeatRepository.UpdateAircraftSeatAsync(existingSeat);
        }

        public async Task<bool> DeleteAircraftSeatAsync(int id)
        {
            return await _aircraftSeatRepository.DeleteAircraftSeatAsync(id);
        }
    }
}
