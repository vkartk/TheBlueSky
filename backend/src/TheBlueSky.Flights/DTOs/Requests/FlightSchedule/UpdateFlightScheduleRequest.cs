using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Flights.DTOs.Requests.FlightSchedule
{
    public record UpdateFlightScheduleRequest(

        [Required] 
        int FlightScheduleId,
        
        [Required] 
        string FlightNumber,
        
        [StringLength(50)]
        string? FlightName,
        
        [Required] 
        TimeOnly DepartureTime,
        
        [Required] 
        TimeOnly ArrivalTime,
        
        [Required] 
        decimal BaseFare,
        
        [Required] 
        int CheckinBaggageWeightKg,
        
        [Required] 
        int CabinBaggageWeightKg,
        
        [Required] 
        DateOnly ValidFrom,
        
        [Required] 
        DateOnly ValidUntil,
        
        bool IsActive
    );
}
