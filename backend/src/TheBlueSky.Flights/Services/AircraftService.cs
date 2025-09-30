using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.Aircraft;
using TheBlueSky.Flights.DTOs.Responses.Aircraft;
using TheBlueSky.Flights.DTOs.Responses.AircraftSeat;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories.Interfaces;

namespace TheBlueSky.Flights.Services
{
    public class AircraftService : IAircraftService
    {
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IMapper _mapper;

        public AircraftService(IAircraftRepository aircraftRepository, IMapper mapper)
        {
            _aircraftRepository = aircraftRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AircraftResponse>> GetAllAircraftsAsync()
        {
            var aircrafts = await _aircraftRepository.GetAllAircraftsAsync();
            return _mapper.Map<IEnumerable<AircraftResponse>>(aircrafts);
        }

        public async Task<AircraftResponse?> GetAircraftByIdAsync(int id)
        {
            var aircraft = await _aircraftRepository.GetAircraftByIdAsync(id);
            return _mapper.Map<AircraftResponse>(aircraft);
        }

        public async Task<AircraftWithSeatsResponse?> GetAircraftWithSeatsByIdAsync(int id)
        {
            var aircraftWithSeats = await _aircraftRepository.GetAircraftWithSeatsByIdAsync(id);
            return _mapper.Map<AircraftWithSeatsResponse>(aircraftWithSeats);
        }

        public async Task<AircraftResponse> CreateAircraftAsync(CreateAircraftRequest request, string ownerId)
        {
            var aircraft = _mapper.Map<Aircraft>(request);
            aircraft.OwnerUserId = ownerId;

            var createdAircraft = await _aircraftRepository.AddAircraftAsync(aircraft);
            return _mapper.Map<AircraftResponse>(createdAircraft);
        }

        public async Task<bool> UpdateAircraftAsync(UpdateAircraftRequest request)
        {
            var existingAircraft = await _aircraftRepository.GetAircraftByIdAsync(request.AircraftId);
            if (existingAircraft == null)
            {
                return false;
            }

            _mapper.Map(request, existingAircraft);
            return await _aircraftRepository.UpdateAircraftAsync(existingAircraft);
        }

        public async Task<bool> DeleteAircraftAsync(int id)
        {
            return await _aircraftRepository.DeleteAircraftAsync(id);
        }

    }
}
