using TheBlueSky.Flights.DTOs.Responses.Aircraft;
using TheBlueSky.Flights.DTOs.Responses.ScheduleDay;

namespace TheBlueSky.Flights.DTOs.Responses.FlightSchedule
{
    public class FlightScheduleWithAircraftResponse
    {
        public int FlightScheduleId { get; set; }
        public int AircraftId { get; set; }
        public int RouteId { get; set; }

        public string FlightNumber { get; set; } = string.Empty;
        public string? FlightName { get; set; }

        public TimeOnly DepartureTime { get; set; }
        public TimeOnly ArrivalTime { get; set; }

        public decimal BaseFare { get; set; }
        public int CheckinBaggageWeightKg { get; set; }
        public int CabinBaggageWeightKg { get; set; }

        public DateOnly ValidFrom { get; set; }
        public DateOnly ValidUntil { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<ScheduleDayResponse> ScheduleDays { get; set; } = new();

        public AircraftResponse? Aircraft { get; set; }
    }
}
