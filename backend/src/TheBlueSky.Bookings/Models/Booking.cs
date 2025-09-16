using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TheBlueSky.Bookings.Enums;

namespace TheBlueSky.Bookings.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int FlightId { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        public int NumberOfPassengers { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal SubtotalAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TaxAmount { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BookingStatus BookingStatus { get; set; } = BookingStatus.Pending;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public Payment? Payment { get; set; }
        public BookingCancellation? Cancellation { get; set; }
        public ICollection<BookingPassenger> Passengers { get; set; } = new List<BookingPassenger>();
        public ICollection<BookingAudit> Audits { get; set; } = new List<BookingAudit>();
    }
}
