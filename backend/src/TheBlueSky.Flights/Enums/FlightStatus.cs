using System.Text.Json.Serialization;

namespace TheBlueSky.Flights.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FlightStatus
    {
        Unknown = 0,
        Scheduled = 1,
        Boarding = 2,
        Departed = 3,
        Arrived = 4,
        Delayed = 5,
        Cancelled = 6,
        Diverted = 7
    }
}
