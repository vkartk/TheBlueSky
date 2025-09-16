using TheBlueSky.Flights.Enums;

namespace TheBlueSky.Flights.DTOs.Responses.FlightSeatStatus
{
    public record FlightSeatStatusResponse(

        int FlightSeatStatusId,

        int FlightId,

        int AircraftSeatId,

        SeatStatus SeatStatus
    );

}
