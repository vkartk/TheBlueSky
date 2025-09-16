using System.ComponentModel.DataAnnotations;
using TheBlueSky.Flights.Enums;

namespace TheBlueSky.Flights.DTOs.Requests.FlightSeatStatus
{
    public record CreateFlightSeatStatusRequest(

        [Required] 
        int FlightId,

        [Required] 
        int AircraftSeatId,

        SeatStatus SeatStatus = SeatStatus.Available
    );

}
