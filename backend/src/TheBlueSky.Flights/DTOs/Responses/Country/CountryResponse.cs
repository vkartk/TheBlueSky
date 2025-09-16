namespace TheBlueSky.Flights.DTOs.Responses.Country
{
    public record CountryResponse(

        string CountryID,

        string CountryName,

        string CurrencyCode,

        bool IsActive
    );

}
