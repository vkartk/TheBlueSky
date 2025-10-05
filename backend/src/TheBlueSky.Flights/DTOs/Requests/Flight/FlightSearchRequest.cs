using System.ComponentModel.DataAnnotations;
using TheBlueSky.Flights.Enums;

namespace TheBlueSky.Flights.DTOs.Requests.Flight
{
    public class FlightSearchRequest
    {
        [Required]
        public int RouteId { get; set; }
        [Required]
        public DateOnly DepartureDate { get; set; }
        public DateOnly? ReturnDate { get; set; }

        [Required]
        public TripType TripType { get; set; }
        public int Adults { get; set; } = 1;

    }
}
