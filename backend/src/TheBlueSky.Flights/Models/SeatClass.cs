using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Flights.Models
{
    public class SeatClass
    {
        public int SeatClassId { get; set; }

        [Required]
        [StringLength(50)]
        public string ClassName { get; set; } = null!;

        [StringLength(255)]
        public string? ClassDescription { get; set; }

        [Required]
        public int PriorityOrder { get; set; }
        public ICollection<AircraftSeat> Seats { get; set; } = new List<AircraftSeat>();

    }
}
