using System.ComponentModel.DataAnnotations;
using TheBlueSky.Flights.Enums;

namespace TheBlueSky.Flights.DTOs.Requests.AircraftSeat
{
    public record UpdateAircraftSeatRequest(

        [Required] 
        int AircraftSeatId,

        [Required]
        [StringLength(10)] 
        string SeatNumber,

        [Required]
        [StringLength(10)] 
        string SeatPosition,

        SeatClass SeatClass,

        [Required]
        decimal AdditionalFare,

        [Required] 
        int SeatRow,

        [Required] 
        int SeatColumn,

        bool IsActive
    );

}
