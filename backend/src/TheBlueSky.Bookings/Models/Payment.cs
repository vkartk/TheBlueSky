using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TheBlueSky.Bookings.Enums;
using TheBlueSky.Bookings.Validation;

namespace TheBlueSky.Bookings.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [Required]
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Unknown;

        [CurrencyAmount]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaymentAmount { get; set; }

        public DateTime? PaymentDate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        [MaxLength(128)]
        public string? GatewayTransactionId { get; set; }

        public DateTime? RefundDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? RefundAmount { get; set; }

    }
}
