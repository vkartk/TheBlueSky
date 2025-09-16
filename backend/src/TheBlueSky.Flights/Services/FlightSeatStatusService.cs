using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.FlightSeatStatus;
using TheBlueSky.Flights.DTOs.Responses.FlightSeatStatus;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories;

namespace TheBlueSky.Flights.Services
{
    public class FlightSeatStatusService : IFlightSeatStatusService
    {
        private readonly IFlightSeatStatusRepository _flightSeatStatusRepository;
        private readonly IMapper _mapper;

        public FlightSeatStatusService(IFlightSeatStatusRepository flightSeatStatusRepository, IMapper mapper)
        {
            _flightSeatStatusRepository = flightSeatStatusRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FlightSeatStatusResponse>> GetAllFlightSeatStatusesAsync()
        {
            var statuses = await _flightSeatStatusRepository.GetAllFlightSeatStatusesAsync();
            return _mapper.Map<IEnumerable<FlightSeatStatusResponse>>(statuses);
        }

        public async Task<FlightSeatStatusResponse?> GetFlightSeatStatusByIdAsync(int id)
        {
            var status = await _flightSeatStatusRepository.GetFlightSeatStatusByIdAsync(id);
            return _mapper.Map<FlightSeatStatusResponse>(status);
        }

        public async Task<FlightSeatStatusResponse> CreateFlightSeatStatusAsync(CreateFlightSeatStatusRequest request)
        {
            var status = _mapper.Map<FlightSeatStatus>(request);
            var createdStatus = await _flightSeatStatusRepository.AddFlightSeatStatusAsync(status);
            return _mapper.Map<FlightSeatStatusResponse>(createdStatus);
        }

        public async Task<bool> UpdateFlightSeatStatusAsync(UpdateFlightSeatStatusRequest request)
        {
            var existingStatus = await _flightSeatStatusRepository.GetFlightSeatStatusByIdAsync(request.FlightSeatStatusId);
            if (existingStatus == null)
            {
                return false;
            }

            _mapper.Map(request, existingStatus);
            return await _flightSeatStatusRepository.UpdateFlightSeatStatusAsync(existingStatus);
        }

        public async Task<bool> DeleteFlightSeatStatusAsync(int id)
        {
            return await _flightSeatStatusRepository.DeleteFlightSeatStatusAsync(id);
        }
    }
}
