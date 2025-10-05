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

            CreateMap<CreateFlightRequest, Flight>()
                .ForMember(d => d.FlightId, opt => opt.Ignore())
                .ForMember(d => d.LastUpdated, opt => opt.Ignore())
                .ForMember(d => d.Schedule, opt => opt.Ignore())
                .ForMember(d => d.SeatStatuses, opt => opt.Ignore());

            CreateMap<UpdateFlightRequest, Flight>()
                .ForMember(d => d.FlightScheduleId, opt => opt.Ignore())
                .ForMember(d => d.LastUpdated, opt => opt.Ignore())
                .ForMember(d => d.Schedule, opt => opt.Ignore())
                .ForMember(d => d.SeatStatuses, opt => opt.Ignore());

            CreateMap<Flight, FlightDetailResponse>()
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.Schedule.Route.OriginAirport))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Schedule.Route.DestinationAirport))
                .ForMember(dest => dest.Aircraft, opt => opt.MapFrom(src => src.Schedule.Aircraft))

                .ForMember(dest => dest.DepartureDateTime, opt => opt.MapFrom(src => src.DepartureDateTime.UtcDateTime))
                .ForMember(dest => dest.ArrivalDateTime, opt => opt.MapFrom(src => src.ArrivalDateTime.UtcDateTime));

        }
    }
}
