using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheBlueSky.Bookings.Enums;

namespace TheBlueSky.Bookings.DTOs.Requests.BookingCancellation
{
    public record CreateBookingCancellationRequest
    {

        [Required]
        public required int BookingId { get; set; }

        [Required]
        public string CancelledByUserId { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal RefundAmount { get; set; }

        [Required]
        public required RefundStatus RefundStatus { get; set; }

        public DateTime? RefundDate { get; set; }

        public string? CancellationReason { get; set; }

        public string? AdminNotes { get; set; }
    };
}
