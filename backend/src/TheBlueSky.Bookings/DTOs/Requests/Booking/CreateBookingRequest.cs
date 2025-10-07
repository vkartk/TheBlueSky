using System.ComponentModel.DataAnnotations;
using TheBlueSky.Bookings.Enums;

namespace TheBlueSky.Bookings.DTOs.Requests.Booking
{
    public record CreateBookingRequest
    {
        [Required]
        public int FlightId { get; set; }

        public string? UserId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Subtotal { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Tax { get; set; }

        [Required]
        [MinLength(1)]
        public List<PassengerSeatSelectionRequest> PassengerSeatSelections { get; set; } = [];
    }
}
