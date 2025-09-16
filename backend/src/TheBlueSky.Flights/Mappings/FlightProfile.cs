using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.Flight;
using TheBlueSky.Flights.DTOs.Responses.Flight;
using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Mappings
{
    public class FlightProfile : Profile
    {
        public FlightProfile()
        {
            CreateMap<Flight, FlightResponse>();
            CreateMap<CreateFlightRequest, Flight>();
            CreateMap<UpdateFlightRequest, Flight>();
        }
    }
}
