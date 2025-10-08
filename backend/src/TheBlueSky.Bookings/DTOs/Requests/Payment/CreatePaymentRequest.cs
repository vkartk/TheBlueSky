using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheBlueSky.Bookings.Enums;

namespace TheBlueSky.Bookings.DTOs.Requests.Payment
{
    public class CreatePaymentRequest
    {
        [Required]
        public required int BookingId { get; set; }

        [Required]
        public required PaymentMethod PaymentMethod { get; set; }

        [Range(0, double.MaxValue)]
        public required decimal PaymentAmount { get; set; }

        public DateTime? PaymentDate { get; set; }

        [Required]
        public required PaymentStatus PaymentStatus { get; set; }

        [MaxLength(128)]
        public string? GatewayTransactionId { get; set; }

        public DateTime? RefundDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? RefundAmount { get; set; }
    }
}
