using System.ComponentModel.DataAnnotations;
using TheBlueSky.Flights.Enums;

namespace TheBlueSky.Flights.Models
{
    public class Flight
    {
        public int FlightId { get; set; }

        [Required]
        public int FlightScheduleId { get; set; }

        [Required]
        public DateOnly FlightDate { get; set; }

        [Required]
        public DateTimeOffset DepartureDateTime { get; set; }

        [Required]
        public DateTimeOffset ArrivalDateTime { get; set; }

        [Required]
        public FlightStatus FlightStatus { get; set; } = FlightStatus.Scheduled;


        public int AvailableSeats { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public FlightSchedule Schedule { get; set; } = null!;
        public ICollection<FlightSeatStatus> SeatStatuses { get; set; } = new List<FlightSeatStatus>();

    }
}
