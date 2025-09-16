using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.ScheduleDay;
using TheBlueSky.Flights.DTOs.Responses.ScheduleDay;
using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Mappings
{
    public class ScheduleDayProfile : Profile
    {
        public ScheduleDayProfile()
        {
            CreateMap<ScheduleDay, ScheduleDayResponse>();
            CreateMap<CreateScheduleDayRequest, ScheduleDay>();
            CreateMap<UpdateScheduleDayRequest, ScheduleDay>();
        }
    }
}
