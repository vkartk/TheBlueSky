using Microsoft.EntityFrameworkCore;
using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly FlightsDbContext _context;

        public CountryRepository(FlightsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<Country> GetCountryByIdAsync(string countryId)
        {
            return await _context.Countries.FindAsync(countryId);
        }



    }
}
