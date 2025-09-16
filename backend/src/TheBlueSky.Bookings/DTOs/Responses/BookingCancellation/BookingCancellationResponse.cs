using TheBlueSky.Bookings.Enums;

namespace TheBlueSky.Bookings.DTOs.Responses.BookingCancellation
{
    public record BookingCancellationResponse(

        int BookingCancellationId,
        int BookingId,

        DateTime CancellationDate,
        int CancelledByUserId,

        decimal RefundAmount,
        RefundStatus RefundStatus,
        DateTime? RefundDate,

        string? CancellationReason,

        string? AdminNotes
    );

}
