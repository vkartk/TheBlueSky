using AutoMapper;
using TheBlueSky.Bookings.DTOs.Requests.Booking;
using TheBlueSky.Bookings.DTOs.Responses.Booking;
using TheBlueSky.Bookings.Models;

namespace TheBlueSky.Bookings.Mappings
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingResponse>()
                .ForMember(d => d.TotalAmount, cfg => cfg.MapFrom(s => s.SubtotalAmount + s.TaxAmount));

            CreateMap<CreateBookingRequest, Booking>()
                .ForMember(d => d.BookingDate, cfg => cfg.MapFrom(_ => DateTime.UtcNow))
                .ForMember(d => d.LastUpdated, cfg => cfg.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UpdateBookingRequest, Booking>()
                .ForMember(d => d.LastUpdated, cfg => cfg.MapFrom(_ => DateTime.UtcNow));
        }

    }
}
