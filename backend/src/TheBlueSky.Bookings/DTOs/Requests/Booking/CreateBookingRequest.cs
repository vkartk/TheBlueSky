using System.ComponentModel.DataAnnotations;
using TheBlueSky.Bookings.Enums;

namespace TheBlueSky.Bookings.DTOs.Requests.Booking
{
    public record CreateBookingRequest
    {
        public required int UserId { get; init; }
        public required int FlightId { get; init; }

        [Range(1, 50)]
        public required int NumberOfPassengers { get; init; }

        [Range(0, double.MaxValue)]
        public required decimal SubtotalAmount { get; init; }

        [Range(0, double.MaxValue)]
        public required decimal TaxAmount { get; init; }

        public BookingStatus BookingStatus { get; init; } = BookingStatus.Pending;
        public PaymentStatus PaymentStatus { get; init; } = PaymentStatus.Pending;
    }
}
