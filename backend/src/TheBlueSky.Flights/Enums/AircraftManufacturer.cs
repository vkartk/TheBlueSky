using System.Text.Json.Serialization;

namespace TheBlueSky.Flights.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AircraftManufacturer
    {
        Unknown = 0,
        Airbus = 1,
        Boeing = 2
    }
}
