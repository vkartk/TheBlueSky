using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Bookings.DTOs.Requests.Flight
{
    public class UpdateSeatStatusRequest
    {
        [Required]
        public int FlightSeatStatusId { get; set; }

        [Required]
        public string SeatStatus { get; set; } = string.Empty;
    }
}
