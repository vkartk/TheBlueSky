using System.Text.Json.Serialization;

namespace TheBlueSky.Flights.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SeatClass
    {
        Economy = 1,
        Business = 2,
        FirstClass = 3
    }
}
