namespace TheBlueSky.Flights.DTOs.Requests.Flight
{
    public class GenerateFlightsRequest
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
