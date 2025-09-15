using System.ComponentModel.DataAnnotations;
using TheBlueSky.Flights.Enums;

namespace TheBlueSky.Flights.Models
{
    public class FlightSeatStatus
    {
        public int FlightSeatStatusId { get; set; }

        [Required]
        public int FlightId { get; set; }

        [Required]
        public int AircraftSeatId { get; set; }

        public SeatStatus SeatStatus { get; set; } = SeatStatus.Available;

        public Flight Flight { get; set; } = null!;
        public AircraftSeat AircraftSeat { get; set; } = null!;

    }
}
