using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.Route;
using TheBlueSky.Flights.DTOs.Responses.Route;
using TheBlueSky.Flights.Repositories;
using Route = TheBlueSky.Flights.Models.Route;

namespace TheBlueSky.Flights.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IMapper _mapper;

        public RouteService(IRouteRepository routeRepository, IMapper mapper)
        {
            _routeRepository = routeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RouteResponse>> GetAllRoutesAsync()
        {
            var routes = await _routeRepository.GetAllRoutesAsync();
            return _mapper.Map<IEnumerable<RouteResponse>>(routes);
        }

        public async Task<RouteResponse?> GetRouteByIdAsync(int id)
        {
            var route = await _routeRepository.GetRouteByIdAsync(id);
            return _mapper.Map<RouteResponse>(route);
        }

        public async Task<RouteResponse> CreateRouteAsync(CreateRouteRequest request)
        {
            var route = _mapper.Map<Route>(request);
            var createdRoute = await _routeRepository.AddRouteAsync(route);
            return _mapper.Map<RouteResponse>(createdRoute);
        }

        public async Task<bool> UpdateRouteAsync(UpdateRouteRequest request)
        {
            var existingRoute = await _routeRepository.GetRouteByIdAsync(request.RouteId);
            if (existingRoute == null)
            {
                return false;
            }

            _mapper.Map(request, existingRoute);
            return await _routeRepository.UpdateRouteAsync(existingRoute);
        }

        public async Task<bool> DeleteRouteAsync(int id)
        {
            return await _routeRepository.DeleteRouteAsync(id);
        }
    }
}
