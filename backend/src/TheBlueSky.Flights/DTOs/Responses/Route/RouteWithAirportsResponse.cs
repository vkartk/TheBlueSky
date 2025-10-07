using TheBlueSky.Flights.DTOs.Responses.Airport;

namespace TheBlueSky.Flights.DTOs.Responses.Route
{
    public class RouteWithAirportsResponse
    {
        public int RouteId { get; set; }
        public int OriginAirportId { get; set; }
        public int DestinationAirportId { get; set; }
        public int DistanceKm { get; set; }
        public int EstimatedDurationMinutes { get; set; }
        public bool IsActive { get; set; }

        public AirportResponse OriginAirport { get; set; } = default!;
        public AirportResponse DestinationAirport { get; set; } = default!;

    }
}
