using System.ComponentModel.DataAnnotations;
using TheBlueSky.Bookings.Enums;

namespace TheBlueSky.Bookings.DTOs.Requests.Booking
{
    public record CreateBookingRequest(

        [Required] 
        int UserId,

        [Required] 
        int FlightId,

        [Range(1, 50)]
        int NumberOfPassengers,

        [Range(0, double.MaxValue)] 
        decimal SubtotalAmount,

        [Range(0, double.MaxValue)] 
        decimal TaxAmount,

        BookingStatus BookingStatus = BookingStatus.Pending,
        PaymentStatus PaymentStatus = PaymentStatus.Pending
    );
}
