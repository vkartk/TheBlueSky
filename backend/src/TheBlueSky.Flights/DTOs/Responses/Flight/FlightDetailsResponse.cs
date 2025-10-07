using TheBlueSky.Flights.DTOs.Responses.FlightSchedule;
using TheBlueSky.Flights.DTOs.Responses.FlightSeatStatus;
using TheBlueSky.Flights.Enums;

namespace TheBlueSky.Flights.DTOs.Responses.Flight
{
    public class FlightDetailsResponse
    {
            public int FlightId { get; set; }
            public int FlightScheduleId { get; set; }
            public DateOnly FlightDate { get; set; }
            public DateTimeOffset DepartureDateTime { get; set; }
            public DateTimeOffset ArrivalDateTime { get; set; }
            public FlightStatus FlightStatus { get; set; }
            public int AvailableSeats { get; set; }
            public decimal BaseFare { get; set; }
            public DateTime LastUpdated { get; set; }

            public FlightScheduleWithAircraftRouteResponse? Schedule { get; set; }
            public List<FlightSeatStatusWithAircraftSeatResponse> SeatStatuses { get; set; } = new();
    }
}
