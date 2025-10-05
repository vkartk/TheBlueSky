using TheBlueSky.Flights.DTOs.Responses.Aircraft;
using TheBlueSky.Flights.DTOs.Responses.Airport;
using TheBlueSky.Flights.Enums;

namespace TheBlueSky.Flights.DTOs.Responses.Flight
{
    public class FlightDetailResponse
    {
        public int FlightId { get; set; }
        public DateTime DepartureDateTime { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public FlightStatus FlightStatus { get; set; }
        public int AvailableSeats { get; set; }
        public decimal BaseFare { get; set; }
        public AirportResponse Origin { get; set; }
        public AirportResponse Destination { get; set; }
        public AircraftResponse Aircraft { get; set; }

    }
}
