using Route = TheBlueSky.Flights.Models.Route;

namespace TheBlueSky.Flights.Services
{
    public interface IRouteService
    {
        Task<IEnumerable<Route>> GetAllRoutesAsync();
        Task<Route?> GetRouteByIdAsync(int id);
        Task<Route> CreateRouteAsync(Route route);
        Task<bool> UpdateRouteAsync(int id, Route route);
        Task<bool> DeleteRouteAsync(int id);

    }
}
