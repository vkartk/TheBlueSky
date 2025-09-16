using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Flights.DTOs.Requests.ScheduleDay
{
    public record UpdateScheduleDayRequest(

        [Required]
        int ScheduleDayId,

        [Required] 
        DayOfWeek DayOfWeek
    );

}
