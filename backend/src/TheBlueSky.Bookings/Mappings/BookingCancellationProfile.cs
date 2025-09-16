using AutoMapper;
using TheBlueSky.Bookings.DTOs.Requests.BookingCancellation;
using TheBlueSky.Bookings.DTOs.Responses.BookingCancellation;
using TheBlueSky.Bookings.Models;

namespace TheBlueSky.Bookings.Mappings
{
    public class BookingCancellationProfile : Profile
    {
        public BookingCancellationProfile()
        {
            CreateMap<BookingCancellation, BookingCancellationResponse>();

            CreateMap<CreateBookingCancellationRequest, BookingCancellation>()
                .ForMember(d => d.BookingCancellationId, opt => opt.Ignore())
                .ForMember(d => d.CancellationDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(d => d.Booking, opt => opt.Ignore());

            CreateMap<UpdateBookingCancellationRequest, BookingCancellation>()
                .ForMember(d => d.Booking, opt => opt.Ignore());
        }

    }
}
