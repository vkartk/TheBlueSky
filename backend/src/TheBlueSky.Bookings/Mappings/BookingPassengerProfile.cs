using AutoMapper;
using TheBlueSky.Bookings.DTOs.Requests.BookingPassenger;
using TheBlueSky.Bookings.DTOs.Responses.BookingPassenger;
using TheBlueSky.Bookings.Models;

namespace TheBlueSky.Bookings.Mappings
{
    public class BookingPassengerProfile : Profile
    {
        public BookingPassengerProfile()
        {
            CreateMap<BookingPassenger, BookingPassengerResponse>();

            CreateMap<CreateBookingPassengerRequest, BookingPassenger>()
                .ForMember(d => d.BookingPassengerId, opt => opt.Ignore())
                .ForMember(d => d.Booking, opt => opt.Ignore())
                .ForMember(d => d.Passenger, opt => opt.Ignore())
                .ForMember(d => d.MealPreference, opt => opt.Ignore());

            CreateMap<UpdateBookingPassengerRequest, BookingPassenger>()
                .ForMember(d => d.Booking, opt => opt.Ignore())
                .ForMember(d => d.Passenger, opt => opt.Ignore())
                .ForMember(d => d.MealPreference, opt => opt.Ignore());
        }

    }
}
