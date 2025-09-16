using System.Text.Json.Serialization;

namespace TheBlueSky.Bookings.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BookingStatus
    {
        Undefined = 0,
        Pending = 1,
        Confirmed = 2,
        Cancelled = 3,
        Completed = 4
    }
}
