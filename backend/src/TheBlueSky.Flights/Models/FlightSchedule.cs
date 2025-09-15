using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBlueSky.Flights.Models
{
    public class FlightSchedule
    {
        public int FlightScheduleId { get; set; }

        [Required]
        public int AircraftId { get; set; }

        [Required]
        public int RouteId { get; set; }

        [Required]
        public string FlightNumber { get; set; } = null!;

        [StringLength(50)]
        public string? FlightName { get; set; }

        [Required]
        public TimeOnly DepartureTime { get; set; }

        [Required]
        public TimeOnly ArrivalTime { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal BaseFare { get; set; }

        [Required]
        [Range(0, 50)]
        public int CheckinBaggageWeightKg { get; set; }

        [Required]
        [Range(0,25)]
        public int CabinBaggageWeightKg { get; set; }

        [Required]
        public DateOnly ValidFrom { get; set; }

        [Required]
        public DateOnly ValidUntil { get; set; }


        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Aircraft Aircraft { get; set; } = null!;
        public Route Route { get; set; } = null!;
        public ICollection<ScheduleDay> Days { get; set; } = new List<ScheduleDay>();
        public ICollection<Flight> Flights { get; set; } = new List<Flight>();

    }
}
