using TheBlueSky.Bookings.Enums;

namespace TheBlueSky.Bookings.DTOs.Responses.Payment
{
    public record PaymentResponse(

        int PaymentId,

        int BookingId,

        PaymentMethod PaymentMethod,
        decimal PaymentAmount,
        DateTime? PaymentDate,
        PaymentStatus PaymentStatus,

        string? GatewayTransactionId,

        DateTime? RefundDate,
        decimal? RefundAmount
    );

}
