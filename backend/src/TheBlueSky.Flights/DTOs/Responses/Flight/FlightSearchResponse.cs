using TheBlueSky.Flights.DTOs.Responses.Aircraft;
using TheBlueSky.Flights.DTOs.Responses.Airport;
using TheBlueSky.Flights.DTOs.Responses.Route;

namespace TheBlueSky.Flights.DTOs.Responses.Flight
{
    public class FlightSearchResponse
    {
        public IEnumerable<FlightDetailResponse> OutboundFlights { get; set; } = new List<FlightDetailResponse>();
        public IEnumerable<FlightDetailResponse>? ReturnFlights { get; set; }

    }
}
