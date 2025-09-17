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

            CreateMap<CreateAircraftSeatRequest, AircraftSeat>()
                .ForMember(d => d.AircraftSeatId, opt => opt.Ignore())
                .ForMember(d => d.IsActive, opt => opt.Ignore())
                .ForMember(d => d.Aircraft, opt => opt.Ignore())
                .ForMember(d => d.SeatClass, opt => opt.Ignore())
                .ForMember(d => d.FlightSeatStatuses, opt => opt.Ignore()); 
            
            CreateMap<UpdateAircraftSeatRequest, AircraftSeat>()
                .ForMember(d => d.AircraftId, opt => opt.Ignore())
                .ForMember(d => d.SeatClassId, opt => opt.Ignore())
                .ForMember(d => d.Aircraft, opt => opt.Ignore())
                .ForMember(d => d.SeatClass, opt => opt.Ignore())
                .ForMember(d => d.FlightSeatStatuses, opt => opt.Ignore());
        }

    }
}
