using TheBlueSky.Bookings.Enums;

namespace TheBlueSky.Bookings.DTOs.Responses.Booking
{
    public record BookingResponse(

        int BookingId,

        string UserId,

        int FlightId,

        DateTime BookingDate,

        int NumberOfPassengers,

        decimal SubtotalAmount,

        decimal TaxAmount,

        decimal TotalAmount,

        BookingStatus BookingStatus,

        PaymentStatus PaymentStatus,

        DateTime LastUpdated

    );
   
}
