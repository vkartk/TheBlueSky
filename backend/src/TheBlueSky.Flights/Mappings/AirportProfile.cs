using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.Airport;
using TheBlueSky.Flights.DTOs.Responses.Airport;
using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Mappings
{
    public class AirportProfile : Profile
    {
        public AirportProfile()
        {
            CreateMap<Airport, AirportResponse>();
            CreateMap<CreateAirportRequest, Airport>();
            CreateMap<UpdateAirportRequest, Airport>();
        }

    }
}
