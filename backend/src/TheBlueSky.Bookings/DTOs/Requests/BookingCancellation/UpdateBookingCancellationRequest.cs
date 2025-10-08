using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheBlueSky.Bookings.Enums;

namespace TheBlueSky.Bookings.DTOs.Requests.BookingCancellation
{
    public class UpdateBookingCancellationRequest
    {
        [Required]
        public required int BookingCancellationId { get; set; }

        [Required]
        public required int BookingId { get; set; }

        [Required]
        public string CancelledByUserId { get; set; } = string.Empty;

        [Required]
        public required DateTime CancellationDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal RefundAmount { get; set; }

        [Required]
        public required RefundStatus RefundStatus { get; set; }

        public DateTime? RefundDate { get; set; }

        public string? CancellationReason { get; set; }

        public string? AdminNotes { get; set; }
    }
}
