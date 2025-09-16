using TheBlueSky.Flights.DTOs.Responses.Country;

namespace TheBlueSky.Flights.Services
{
    public interface ICountryService
    {
        Task<IEnumerable<CountryResponse>> GetAllCountriesAsync();
        Task<CountryResponse?> GetCountryByIdAsync(string countryId);

    }
}
