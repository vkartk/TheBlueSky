namespace TheBlueSky.Flights.DTOs.Responses.FlightSchedule
{
    public record FlightScheduleResponse(

        int FlightScheduleId,

        int AircraftId,

        int RouteId,

        string FlightNumber,

        string? FlightName,

        TimeOnly DepartureTime,

        TimeOnly ArrivalTime,

        decimal BaseFare,

        int CheckinBaggageWeightKg,

        int CabinBaggageWeightKg,

        DateOnly ValidFrom,

        DateOnly ValidUntil,

        bool IsActive,

        DateTime CreatedDate
    );

}
