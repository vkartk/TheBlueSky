namespace TheBlueSky.Bookings.Models
{
    public class BookingAudit
    {
        public int BookingAuditId { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        public string ActionType { get; set; } = "Unknown";
        public string? OldValuesJson { get; set; }
        public string? NewValuesJson { get; set; }
        public int PerformedByUserId { get; set; }
        public DateTime AuditTimestamp { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }

    }
}
