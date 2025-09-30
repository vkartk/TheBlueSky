using TheBlueSky.Flights.DTOs.Responses.AircraftSeat;

namespace TheBlueSky.Flights.DTOs.Responses.Aircraft
{
    public class AircraftWithSeatsResponse
    {
        public AircraftResponse Aircraft { get; set; } = null!;
        public IEnumerable<AircraftSeatResponse> Seats { get; set; } = new List<AircraftSeatResponse>();

    }
}
