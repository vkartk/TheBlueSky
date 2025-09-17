using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.FlightSchedule;
using TheBlueSky.Flights.DTOs.Responses.FlightSchedule;
using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Mappings
{
    public class FlightScheduleProfile : Profile
    {
        public FlightScheduleProfile()
        {
            CreateMap<FlightSchedule, FlightScheduleResponse>();

            CreateMap<CreateFlightScheduleRequest, FlightSchedule>()
                .ForMember(d => d.FlightScheduleId, opt => opt.Ignore())
                .ForMember(d => d.IsActive, opt => opt.Ignore())
                .ForMember(d => d.CreatedDate, opt => opt.Ignore())
                .ForMember(d => d.Aircraft, opt => opt.Ignore())
                .ForMember(d => d.Route, opt => opt.Ignore())
                .ForMember(d => d.Days, opt => opt.Ignore())
                .ForMember(d => d.Flights, opt => opt.Ignore());
           
            CreateMap<UpdateFlightScheduleRequest, FlightSchedule>()
                .ForMember(d => d.AircraftId, opt => opt.Ignore())
                .ForMember(d => d.RouteId, opt => opt.Ignore())
                .ForMember(d => d.CreatedDate, opt => opt.Ignore())
                .ForMember(d => d.Aircraft, opt => opt.Ignore())
                .ForMember(d => d.Route, opt => opt.Ignore())
                .ForMember(d => d.Days, opt => opt.Ignore())
                .ForMember(d => d.Flights, opt => opt.Ignore());
        }

    }
}
