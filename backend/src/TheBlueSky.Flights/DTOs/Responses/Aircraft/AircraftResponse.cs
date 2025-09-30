using TheBlueSky.Flights.Enums;

namespace TheBlueSky.Flights.DTOs.Responses.Aircraft
{
    public record AircraftResponse(

        int AircraftId,

        string OwnerUserId,

        string AircraftName,

        string AircraftModel,

        AircraftManufacturer Manufacturer,

        int EconomySeats,

        int BusinessSeats,

        int FirstClassSeats,

        bool IsActive,

        DateTime CreatedDate
    );

}
