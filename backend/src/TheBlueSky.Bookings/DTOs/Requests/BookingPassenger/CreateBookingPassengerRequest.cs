using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Bookings.DTOs.Requests.BookingPassenger
{
    public class CreateBookingPassengerRequest
    {
        [Required]
        public required int BookingId { get; set; }

        [Required]
        public required int PassengerId { get; set; }

        [Required]
        public required int FlightSeatStatusId { get; set; }

        [Required]
        [MaxLength(32)]
        public string TicketNumber { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal TicketPrice { get; set; }

        public int? MealPreferenceId { get; set; }
    }


}
