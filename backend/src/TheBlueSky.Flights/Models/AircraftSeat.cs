using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheBlueSky.Flights.Enums;

namespace TheBlueSky.Flights.Models
{
    public class AircraftSeat
    {
        public int AircraftSeatId { get; set; }

        [Required]
        [ForeignKey("Aircraft")]
        public int AircraftId { get; set; }

        [Required]
        [StringLength(10)]
        public string SeatNumber { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string SeatPosition { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal AdditionalFare { get; set; }

        [Required]
        public int SeatRow { get; set; }

        [Required]
        public int SeatColumn { get; set; }

        public bool IsActive { get; set; } = true;

        public Aircraft Aircraft { get; set; } = null!;
        public SeatClass SeatClass { get; set; }
        public ICollection<FlightSeatStatus> FlightSeatStatuses { get; set; } = new List<FlightSeatStatus>();

    }
}
