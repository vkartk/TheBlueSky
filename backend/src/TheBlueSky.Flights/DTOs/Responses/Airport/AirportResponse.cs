namespace TheBlueSky.Flights.DTOs.Responses.Airport
{
    public record AirportResponse(

        int AirportId,

        string AirportCode,

        string AirportName,

        string City,

        string CountryId,

        bool IsActive
    );
}
