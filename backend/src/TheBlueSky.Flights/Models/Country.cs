using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBlueSky.Flights.Models
{
    public class Country
    {
        [Key]
        [MaxLength(2)]
        [Column(TypeName = "char(2)")]
        public string CountryID { get; set; } = default!;

        [Required, MaxLength(64)]
        public string CountryName { get; set; } = default!;

        [Required, MaxLength(3)]
        [Column(TypeName = "char(3)")]
        public string CurrencyCode { get; set; } = default!;

        public bool isActive { get; set; } = true;

        public ICollection<Airport> Airports { get; set; } = new List<Airport>();
    }
}
