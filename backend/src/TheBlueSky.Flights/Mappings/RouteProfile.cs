using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.Route;
using TheBlueSky.Flights.DTOs.Responses.Route;

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
