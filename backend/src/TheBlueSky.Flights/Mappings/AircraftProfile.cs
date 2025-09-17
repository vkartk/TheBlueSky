using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.Aircraft;
using TheBlueSky.Flights.DTOs.Responses.Aircraft;
using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Mappings
{
    public class AircraftProfile : Profile
    {
        public AircraftProfile()
        {
            CreateMap<Aircraft, AircraftResponse>();
            CreateMap<CreateAircraftRequest, Aircraft>()
                .ForMember(d => d.AircraftId, opt => opt.Ignore())
                .ForMember(d => d.IsActive, opt => opt.Ignore())
                .ForMember(d => d.CreatedDate, opt => opt.Ignore())
                .ForMember(d => d.Seats, opt => opt.Ignore())
                .ForMember(d => d.Schedules, opt => opt.Ignore());

            CreateMap<UpdateAircraftRequest, Aircraft>()
                .ForMember(d => d.OwnerUserId, opt => opt.Ignore())
                .ForMember(d => d.CreatedDate, opt => opt.Ignore())
                .ForMember(d => d.Seats, opt => opt.Ignore())
                .ForMember(d => d.Schedules, opt => opt.Ignore()); ;
        }

    }
}
