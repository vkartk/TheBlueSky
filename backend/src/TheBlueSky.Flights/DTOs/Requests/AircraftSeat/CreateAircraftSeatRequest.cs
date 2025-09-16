using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Flights.DTOs.Requests.AircraftSeat
{
    public record CreateAircraftSeatRequest(

        [Required] 
        int AircraftId,

        [Required]
        int SeatClassId,

        [Required]
        [StringLength(10)] 
        string SeatNumber,

        [Required]
        [StringLength(10)] 
        string SeatPosition,

        [Required] 
        decimal AdditionalFare,

        [Required] 
        int SeatRow,

        [Required] 
        int SeatColumn
    );

}
