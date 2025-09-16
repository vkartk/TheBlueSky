using System.Text.Json.Serialization;

namespace TheBlueSky.Bookings.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentStatus
    {
        Undefined = 0,
        Pending = 1,
        Paid = 2,
        Failed = 3,
        Refunded = 4
    }
}
