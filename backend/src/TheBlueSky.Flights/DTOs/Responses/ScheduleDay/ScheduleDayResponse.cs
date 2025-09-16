namespace TheBlueSky.Flights.DTOs.Responses.ScheduleDay
{
    public record ScheduleDayResponse(

        int ScheduleDayId,

        int FlightScheduleId,

        DayOfWeek DayOfWeek
    );

}
