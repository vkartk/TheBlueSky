using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBlueSky.Bookings.Models
{
    public class BookingPassenger
    {
        public int BookingPassengerId { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        public int PassengerId { get; set; }
        public Passenger Passenger { get; set; } = null!;

        public int FlightSeatStatusId { get; set; }

        [MaxLength(32)]
        public string TicketNumber { get; set; } = default!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TicketPrice { get; set; }

        public int? MealPreferenceId { get; set; }
        public MealPreference? MealPreference { get; set; }
    }
}
