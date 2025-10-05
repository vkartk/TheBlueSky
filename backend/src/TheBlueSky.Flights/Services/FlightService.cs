using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.Flight;
using TheBlueSky.Flights.DTOs.Responses.Flight;
using TheBlueSky.Flights.Enums;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories;
using TheBlueSky.Flights.Repositories.Interfaces;

namespace TheBlueSky.Flights.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IRouteRepository _routeRepository;
        private readonly IMapper _mapper;

        public FlightService(IFlightRepository flightRepository, IRouteRepository routeRepository, IMapper mapper)
        {
            _flightRepository = flightRepository;
            _routeRepository = routeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FlightResponse>> GetAllFlightsAsync()
        {
            var flights = await _flightRepository.GetAllFlightsAsync();
            return _mapper.Map<IEnumerable<FlightResponse>>(flights);
        }

        public async Task<FlightResponse?> GetFlightByIdAsync(int id)
        {
            var flight = await _flightRepository.GetFlightByIdAsync(id);
            return _mapper.Map<FlightResponse>(flight);
        }

        public async Task<FlightResponse> CreateFlightAsync(CreateFlightRequest request)
        {
            var flight = _mapper.Map<Flight>(request);
            flight.LastUpdated = DateTime.UtcNow;
            var createdFlight = await _flightRepository.AddFlightAsync(flight);
            return _mapper.Map<FlightResponse>(createdFlight);
        }

        public async Task<bool> UpdateFlightAsync(UpdateFlightRequest request)
        {
            var existingFlight = await _flightRepository.GetFlightByIdAsync(request.FlightId);
            if (existingFlight == null)
            {
                return false;
            }

            _mapper.Map(request, existingFlight);
            existingFlight.LastUpdated = DateTime.UtcNow;
            return await _flightRepository.UpdateFlightAsync(existingFlight);
        }

        public async Task<bool> DeleteFlightAsync(int id)
        {
            return await _flightRepository.DeleteFlightAsync(id);
        }

        public async Task<FlightSearchResponse> SearchFlightsAsync(FlightSearchRequest request)
        {
            var response = new FlightSearchResponse();

            var outboundFlights = await _flightRepository.SearchFlightsAsync(request.RouteId, request.DepartureDate, request.Adults);
            response.OutboundFlights = _mapper.Map<IEnumerable<FlightDetailResponse>>(outboundFlights);

            if (request.TripType == TripType.RoundTrip && request.ReturnDate.HasValue)
            {
                var reverseRoute = await _routeRepository.GetReverseRouteAsync(request.RouteId);
                if (reverseRoute != null)
                {
                    var returnFlights = await _flightRepository.SearchFlightsAsync(reverseRoute.RouteId, request.ReturnDate.Value, request.Adults);
                    response.ReturnFlights = _mapper.Map<IEnumerable<FlightDetailResponse>>(returnFlights);
                }
            }
            return response;
        }

    }
}
