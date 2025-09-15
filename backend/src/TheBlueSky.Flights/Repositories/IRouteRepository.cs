using Route = TheBlueSky.Flights.Models.Route;

namespace TheBlueSky.Flights.Repositories
{
    public interface IRouteRepository
    {
        Task<IEnumerable<Route>> GetAllRoutesAsync();
        Task<Route?> GetRouteByIdAsync(int id);
        Task AddRouteAsync(Route route);
        Task UpdateRouteAsync(Route route);
        Task DeleteRouteAsync(int id);
        Task<bool> RouteExists(int id);

    }
}
