using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Services
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAllCountries();
        Task<Country> GetCountryById(string countryId);

    }
}
