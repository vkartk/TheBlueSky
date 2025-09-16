using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TheBlueSky.Bookings.Enums;

namespace TheBlueSky.Bookings.Models
{
    public class BookingCancellation
    {
        public int BookingCancellationId { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        public DateTime CancellationDate { get; set; } = DateTime.UtcNow;
        public int CancelledByUserId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal RefundAmount { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RefundStatus RefundStatus { get; set; } = RefundStatus.None;

        public DateTime? RefundDate { get; set; }

        public string? CancellationReason { get; set; }
        public string? AdminNotes { get; set; }

    }
}
