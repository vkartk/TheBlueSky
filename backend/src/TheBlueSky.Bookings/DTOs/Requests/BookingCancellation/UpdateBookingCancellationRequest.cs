using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheBlueSky.Bookings.Enums;

namespace TheBlueSky.Bookings.DTOs.Requests.BookingCancellation
{
    public record UpdateBookingCancellationRequest(

        [Required]
        int BookingCancellationId,

        [Required]
        int BookingId,

        [Required]
        int CancelledByUserId,

        [Required]
        DateTime CancellationDate,

        [Range(0, double.MaxValue)]
        decimal RefundAmount,

        [Required]
        RefundStatus RefundStatus,

        DateTime? RefundDate,

        string? CancellationReason,

        string? AdminNotes
    );
}
