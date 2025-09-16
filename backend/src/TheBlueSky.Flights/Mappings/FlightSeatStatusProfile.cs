using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.FlightSeatStatus;
using TheBlueSky.Flights.DTOs.Responses.FlightSeatStatus;
using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Mappings
{
    public class FlightSeatStatusProfile : Profile
    {
        public FlightSeatStatusProfile()
        {
            CreateMap<FlightSeatStatus, FlightSeatStatusResponse>();
            CreateMap<CreateFlightSeatStatusRequest, FlightSeatStatus>();
            CreateMap<UpdateFlightSeatStatusRequest, FlightSeatStatus>();
        }

    }
}
