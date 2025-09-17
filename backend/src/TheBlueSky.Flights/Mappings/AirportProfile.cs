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

            CreateMap<CreateAirportRequest, Airport>()
                .ForMember(d => d.AirportId, opt => opt.Ignore())
                .ForMember(d => d.IsActive, opt => opt.Ignore())
                .ForMember(d => d.Country, opt => opt.Ignore())
                .ForMember(d => d.OriginRoutes, opt => opt.Ignore())
                .ForMember(d => d.DestinationRoutes, opt => opt.Ignore());

            CreateMap<UpdateAirportRequest, Airport>()
                .ForMember(d => d.Country, opt => opt.Ignore())
                .ForMember(d => d.OriginRoutes, opt => opt.Ignore())
                .ForMember(d => d.DestinationRoutes, opt => opt.Ignore());
        }

    }
}
