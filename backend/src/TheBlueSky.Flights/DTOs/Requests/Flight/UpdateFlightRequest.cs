using System.ComponentModel.DataAnnotations;
using TheBlueSky.Flights.Enums;

namespace TheBlueSky.Flights.DTOs.Requests.Flight
{
    public record UpdateFlightRequest(

        [Required] 
        int FlightId,

        [Required] 
        DateOnly FlightDate,

        [Required] 
        DateTimeOffset DepartureDateTime,

        [Required] 
        DateTimeOffset ArrivalDateTime,

        [Required] 
        FlightStatus FlightStatus,

        int AvailableSeats
    );

}
