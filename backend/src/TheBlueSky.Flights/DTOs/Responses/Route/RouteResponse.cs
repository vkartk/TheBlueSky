namespace TheBlueSky.Flights.DTOs.Responses.Route
{
    public record RouteResponse(

        int RouteId,

        int OriginAirportId,

        int DestinationAirportId,

        int DistanceKm,

        int EstimatedDurationMinutes,

        bool IsActive
    );

}
