using AutoMapper;
using TheBlueSky.Bookings.DTOs.Requests.Payment;
using TheBlueSky.Bookings.DTOs.Responses.Payment;
using TheBlueSky.Bookings.Models;

namespace TheBlueSky.Bookings.Mappings
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentResponse>();

            CreateMap<CreatePaymentRequest, Payment>()
                .ForMember(d => d.PaymentId, opt => opt.Ignore())
                .ForMember(d => d.Booking, opt => opt.Ignore());

            CreateMap<UpdatePaymentRequest, Payment>()
                .ForMember(d => d.Booking, opt => opt.Ignore());
        }
    }
}
