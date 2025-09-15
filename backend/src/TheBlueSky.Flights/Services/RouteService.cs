using TheBlueSky.Flights.Repositories;
using Route = TheBlueSky.Flights.Models.Route;

namespace TheBlueSky.Flights.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRouteRepository _routeRepository;

        public RouteService(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public async Task<IEnumerable<Route>> GetAllRoutesAsync()
        {
            return await _routeRepository.GetAllRoutesAsync();
        }

        public async Task<Route?> GetRouteByIdAsync(int id)
        {
            return await _routeRepository.GetRouteByIdAsync(id);
        }

        public async Task<Route> CreateRouteAsync(Route route)
        {
            await _routeRepository.AddRouteAsync(route);
            return route;
        }

        public async Task<bool> UpdateRouteAsync(int id, Route route)
        {
            if (id != route.RouteId)
            {
                return false;
            }

            var exists = await _routeRepository.RouteExists(id);
            if (!exists)
            {
                return false;
            }

            await _routeRepository.UpdateRouteAsync(route);
            return true;
        }

        public async Task<bool> DeleteRouteAsync(int id)
        {
            var exists = await _routeRepository.RouteExists(id);
            if (!exists)
            {
                return false;
            }

            await _routeRepository.DeleteRouteAsync(id);
            return true;
        }
    }
}
