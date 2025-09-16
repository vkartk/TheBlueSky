using System.ComponentModel.DataAnnotations;
using TheBlueSky.Flights.Enums;

namespace TheBlueSky.Flights.DTOs.Requests.FlightSeatStatus
{
    public record UpdateFlightSeatStatusRequest(

        [Required] 
        int FlightSeatStatusId,

        [Required] 
        SeatStatus SeatStatus
    );

}
