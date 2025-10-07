using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheBlueSky.Bookings.Enums;

namespace TheBlueSky.Bookings.DTOs.Requests.BookingCancellation
{
    public record CreateBookingCancellationRequest(

        [Required]
        int BookingId,

        [Required]
        string CancelledByUserId,

        [Range(0, double.MaxValue)]
        decimal RefundAmount,

        [Required]
        RefundStatus RefundStatus,

        DateTime? RefundDate,

        string? CancellationReason,

        string? AdminNotes
    );
}
