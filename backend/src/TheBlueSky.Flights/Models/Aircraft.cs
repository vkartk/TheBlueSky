using System.ComponentModel.DataAnnotations;
using TheBlueSky.Flights.Enums;

namespace TheBlueSky.Flights.Models
{
    public class Aircraft
    {
        public int AircraftId { get; set; }

        [Required(ErrorMessage = "Flight Owner user ID is required.")]
        public int OwnerUserId { get; set; }

        [Required(ErrorMessage = "Aircraft name is required.")]
        [StringLength(100, ErrorMessage = "Aircraft name cannot exceed 100 characters.")]
        public string AircraftName { get; set; } = default!;

        [Required(ErrorMessage = "Aircraft model is required.")]
        [StringLength(100, ErrorMessage = "Aircraft model cannot exceed 100 characters.")]
        public string AircraftModel { get; set; } = default!;

        public AircraftManufacturer Manufacturer { get; set; } = AircraftManufacturer.Unknown;

        [Required(ErrorMessage = "Number of economy class seats is required.")]
        public int EconomySeats { get; set; }

        [Required(ErrorMessage = "Number of business class seats is required.")]
        public int BusinessSeats { get; set; }

        [Required(ErrorMessage = "Number of first class seats is required.")]
        public int FirstClassSeats { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<AircraftSeat> Seats { get; set; } = new List<AircraftSeat>();
        public ICollection<FlightSchedule> Schedules { get; set; } = new List<FlightSchedule>();


    }
}
