using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Flights.Models
{
    public class ScheduleDay
    {
        public int ScheduleDayId { get; set; }

        [Required]
        public int ScheduleId { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        public FlightSchedule Schedule { get; set; } = null!;
    }
}
