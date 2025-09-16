using Microsoft.EntityFrameworkCore;
using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Data.Seeders
{
    public class CountrySeeder : IDataSeeder
    {
        private readonly FlightsDbContext _context;

        public CountrySeeder(FlightsDbContext context)
        {
            _context = context;
        }

        public int Priority => 1;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            var countriesToSeed = new List<Country>
            {
                new Country { CountryID = "US", CountryName = "United States", CurrencyCode = "USD", isActive = true },
                new Country { CountryID = "GB", CountryName = "United Kingdom", CurrencyCode = "GBP", isActive = true },
                new Country { CountryID = "CA", CountryName = "Canada", CurrencyCode = "CAD", isActive = true },
                new Country { CountryID = "AU", CountryName = "Australia", CurrencyCode = "AUD", isActive = true },
                new Country { CountryID = "IN", CountryName = "India", CurrencyCode = "INR", isActive = true },
                new Country { CountryID = "DE", CountryName = "Germany", CurrencyCode = "EUR", isActive = true },
                new Country { CountryID = "FR", CountryName = "France", CurrencyCode = "EUR", isActive = true },
                new Country { CountryID = "JP", CountryName = "Japan", CurrencyCode = "JPY", isActive = true },
            };

            var existingCountryIdsList = await _context.Countries
                .Select(c => c.CountryID)
                .ToListAsync(cancellationToken);

            var existingCountryIds = new HashSet<string>(existingCountryIdsList);

            var newCountries = countriesToSeed
                .Where(c => !existingCountryIds.Contains(c.CountryID))
                .ToList();

            
            if (newCountries.Any())
            {
                await _context.Countries.AddRangeAsync(newCountries, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
