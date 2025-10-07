using TheBlueSky.Flights.DTOs.Responses.AircraftSeat;
using TheBlueSky.Flights.Enums;

namespace TheBlueSky.Flights.DTOs.Responses.FlightSeatStatus
{
    public class FlightSeatStatusWithAircraftSeatResponse
    {
        public int FlightSeatStatusId { get; set; }
        public int FlightId { get; set; }
        public int AircraftSeatId { get; set; }
        public SeatStatus SeatStatus { get; set; }
        public AircraftSeatResponse? AircraftSeat { get; set; }

    }
}
