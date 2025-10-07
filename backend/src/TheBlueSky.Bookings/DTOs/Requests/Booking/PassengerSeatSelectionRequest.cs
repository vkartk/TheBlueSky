using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Bookings.DTOs.Requests.Booking
{
    public class PassengerSeatSelectionRequest
    {
        [Required]
        public int PassengerId { get; set; }

        [Required]
        public int FlightSeatStatusId { get; set; }

        [Required]
        public string TicketNumber { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal TicketPrice { get; set; }
    }
}
