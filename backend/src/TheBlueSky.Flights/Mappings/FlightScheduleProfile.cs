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
            CreateMap<CreateFlightScheduleRequest, FlightSchedule>();
            CreateMap<UpdateFlightScheduleRequest, FlightSchedule>();
        }

    }
}
