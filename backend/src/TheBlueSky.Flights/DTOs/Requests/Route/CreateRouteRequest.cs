using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Flights.DTOs.Requests.Route
{
    public record CreateRouteRequest(
        
        [Required] 
        int OriginAirportId,
        
        [Required] 
        int DestinationAirportId,
        
        [Required]
        [Range(0, 50000)] 
        int DistanceKm,
        
        [Required]
        [Range(0, 2880)] 
        int EstimatedDurationMinutes
    );

}
