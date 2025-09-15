using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBlueSky.Flights.Models
{
    public class Airport
    {
        public int AirportId { get; set; }

        [Required]
        [StringLength(3)]
        public string AirportCode { get; set; } = default!;

        [Required]
        [StringLength(200)]
        public string AirportName { get; set; } = default!;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = default!;

        [ForeignKey("Country")]
        public string CountryId { get; set; } = default!;

        public bool IsActive { get; set; } = true;

        public Country Country { get; set; } = null!;
        public ICollection<Route> OriginRoutes { get; set; } = new List<Route>();
        public ICollection<Route> DestinationRoutes { get; set; } = new List<Route>();

    }
}
