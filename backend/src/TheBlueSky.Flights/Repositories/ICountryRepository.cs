using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Repositories
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<Country> GetCountryByIdAsync(string countryId);

    }
}
