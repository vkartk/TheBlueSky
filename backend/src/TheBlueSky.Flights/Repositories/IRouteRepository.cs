using Route = TheBlueSky.Flights.Models.Route;

namespace TheBlueSky.Flights.Repositories
{
    public interface IRouteRepository
    {
        Task<IEnumerable<Route>> GetAllRoutesAsync();
        Task<Route?> GetRouteByIdAsync(int id);
        Task<Route> AddRouteAsync(Route route);
        Task<bool> UpdateRouteAsync(Route route);
        Task<bool> DeleteRouteAsync(int id);
        Task<bool> ExistsAsync(int id);

    }
}
