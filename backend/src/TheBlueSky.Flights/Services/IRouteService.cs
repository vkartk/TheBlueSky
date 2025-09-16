using TheBlueSky.Flights.DTOs.Requests.Route;
using TheBlueSky.Flights.DTOs.Responses.Route;
using Route = TheBlueSky.Flights.Models.Route;

namespace TheBlueSky.Flights.Services
{
    public interface IRouteService
    {
        Task<IEnumerable<RouteResponse>> GetAllRoutesAsync();
        Task<RouteResponse?> GetRouteByIdAsync(int id);
        Task<RouteResponse> CreateRouteAsync(CreateRouteRequest request);
        Task<bool> UpdateRouteAsync(UpdateRouteRequest request);
        Task<bool> DeleteRouteAsync(int id);

    }
}
