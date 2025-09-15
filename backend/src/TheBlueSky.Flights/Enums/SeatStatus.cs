using System.Text.Json.Serialization;

namespace TheBlueSky.Flights.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SeatStatus
    {
        Unknown = 0, 
        Available = 1,
        Hold = 2,
        Reserved = 3, 
        CheckedIn = 4,
        Boarded = 5,
        Blocked = 6, 
        Inoperative = 7
    }
}
