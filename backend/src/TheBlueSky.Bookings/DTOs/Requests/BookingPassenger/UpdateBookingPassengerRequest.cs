using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBlueSky.Bookings.DTOs.Requests.BookingPassenger
{
    public class UpdateBookingPassengerRequest
    {
        [Required]
        public required int BookingPassengerId { get; set; }

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
