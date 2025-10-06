using TheBlueSky.Flights.Enums;

namespace TheBlueSky.Flights.DTOs.Responses.AircraftSeat
{
    public record AircraftSeatResponse(

        int AircraftSeatId,

        int AircraftId,

        SeatClass SeatClass,

        string SeatNumber,

        string SeatPosition,

        decimal AdditionalFare,

        int SeatRow,

        int SeatColumn,

        bool IsActive
    );

}
