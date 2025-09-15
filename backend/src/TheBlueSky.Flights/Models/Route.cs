using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Flights.Models
{
    public class Route
    {
        public int RouteId { get; set; }

        [Required]
        public int OriginAirportId { get; set; }

        [Required]
        public int DestinationAirportId { get; set; }

        [Required]
        [Range(0, 50000)]
        public int DistanceKm { get; set; }

        [Required]
        [Range(0, 2880)]
        public int EstimatedDurationMinutes { get; set; }
        public bool IsActive { get; set; } = true;

        public Airport OriginAirport { get; set; } = null!;
        public Airport DestinationAirport { get; set; } = null!;

        public ICollection<FlightSchedule> FlightSchedules { get; set; } = new List<FlightSchedule>();

    }
}
