using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.Route;
using TheBlueSky.Flights.DTOs.Responses.Route;
using Route = TheBlueSky.Flights.Models.Route;

namespace TheBlueSky.Flights.Mappings
{
    public class RouteProfile : Profile
    {
        public RouteProfile()
        {
            CreateMap<Route, RouteResponse>();
            CreateMap<CreateRouteRequest, Route>();
            CreateMap<UpdateRouteRequest, Route>();
        }

    }
}
