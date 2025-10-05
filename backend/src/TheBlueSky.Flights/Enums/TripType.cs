using System.Text.Json.Serialization;

namespace TheBlueSky.Flights.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TripType
    {
        OneWay = 1,
        RoundTrip = 2
    }
}
