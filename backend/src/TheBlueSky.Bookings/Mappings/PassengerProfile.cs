using AutoMapper;
using TheBlueSky.Bookings.DTOs.Requests.Passenger;
using TheBlueSky.Bookings.DTOs.Responses.Passenger;
using TheBlueSky.Bookings.Models;

namespace TheBlueSky.Bookings.Mappings
{
    public class PassengerProfile : Profile
    {
        public PassengerProfile()
        {
            CreateMap<Passenger, PassengerResponse>();

            CreateMap<CreatePassengerRequest, Passenger>()
                .ForMember(d => d.PassengerId, opt => opt.Ignore())
                .ForMember(d => d.CreatedDate, opt => opt.MapFrom( o => DateTime.UtcNow))
                .ForMember(d => d.IsActive, opt => opt.MapFrom( o => true))
                .ForMember(d => d.Bookings, opt => opt.Ignore());

            CreateMap<UpdatePassengerRequest, Passenger>()
                .ForMember(d => d.CreatedDate, opt => opt.Ignore())
                .ForMember(d => d.Bookings, opt => opt.Ignore());
        }

    }
}
