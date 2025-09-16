using System.Text.Json.Serialization;

namespace TheBlueSky.Bookings.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RefundStatus
    {
        None = 0,
        Pending = 1,
        Processed = 2,
        Failed = 3
    }
}
