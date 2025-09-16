using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Flights.DTOs.Requests.ScheduleDay
{
    public record CreateScheduleDayRequest(

        [Required] 
        int FlightScheduleId,

        [Required] 
        DayOfWeek DayOfWeek
    );

}
