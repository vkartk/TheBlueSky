using TheBlueSky.Flights.Enums;

namespace TheBlueSky.Flights.DTOs.Responses.Flight
{
    public record FlightResponse(

        int FlightId,

        int FlightScheduleId,

        DateOnly FlightDate,

        DateTimeOffset DepartureDateTime,

        DateTimeOffset ArrivalDateTime,

        FlightStatus FlightStatus,

        int AvailableSeats,

        DateTime LastUpdated
    );

}
