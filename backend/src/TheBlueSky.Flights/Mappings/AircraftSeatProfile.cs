using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.AircraftSeat;
using TheBlueSky.Flights.DTOs.Responses.AircraftSeat;
using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Mappings
{
    public class AircraftSeatProfile : Profile
    {
        public AircraftSeatProfile()
        {
            CreateMap<AircraftSeat, AircraftSeatResponse>();
            CreateMap<CreateAircraftSeatRequest, AircraftSeat>();
            CreateMap<UpdateAircraftSeatRequest, AircraftSeat>();
        }

    }
}
