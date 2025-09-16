using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheBlueSky.Bookings.Enums;

namespace TheBlueSky.Bookings.DTOs.Requests.Payment
{
    public record UpdatePaymentRequest(

        [Required]
        int PaymentId,

        [Required]
        int BookingId,

        [Required]
        PaymentStatus PaymentMethod,

        [Range(0, double.MaxValue)]
        decimal PaymentAmount,

        DateTime? PaymentDate,

        [Required]
        PaymentStatus PaymentStatus,

        [MaxLength(128)]
        string? GatewayTransactionId,

        DateTime? RefundDate,

        [Range(0, double.MaxValue)]
        decimal? RefundAmount
    );

}
