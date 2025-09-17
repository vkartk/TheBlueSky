using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.Airport;
using TheBlueSky.Flights.DTOs.Responses.Airport;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories.Interfaces;

namespace TheBlueSky.Flights.Services
{
    public class AirportService : IAirportService
    {
        private readonly IAirportRepository _airportRepository;
        private readonly IMapper _mapper;

        public AirportService(IAirportRepository airportRepository, IMapper mapper)
        {
            _airportRepository = airportRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AirportResponse>> GetAllAirportsAsync()
        {
            var airports = await _airportRepository.GetAllAirportsAsync();
            return _mapper.Map<IEnumerable<AirportResponse>>(airports);
        }

        public async Task<AirportResponse?> GetAirportByIdAsync(int id)
        {
            var airport = await _airportRepository.GetAirportByIdAsync(id);
            return _mapper.Map<AirportResponse>(airport);
        }

        public async Task<AirportResponse> CreateAirportAsync(CreateAirportRequest request)
        {
            var airport = _mapper.Map<Airport>(request);
            var createdAirport = await _airportRepository.AddAirportAsync(airport);
            return _mapper.Map<AirportResponse>(createdAirport);
        }

        public async Task<bool> UpdateAirportAsync(UpdateAirportRequest request)
        {
            var existingAirport = await _airportRepository.GetAirportByIdAsync(request.AirportId);
            if (existingAirport == null)
            {
                return false;
            }

            _mapper.Map(request, existingAirport);
            return await _airportRepository.UpdateAirportAsync(existingAirport);
        }

        public async Task<bool> DeleteAirportAsync(int id)
        {
            return await _airportRepository.DeleteAirportAsync(id);
        }

    }
}
