using Microsoft.EntityFrameworkCore;
using System.Text.Json;
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
            var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "countries.json");

            if (!File.Exists(filePath))
                return;

            var json = await File.ReadAllTextAsync(filePath, cancellationToken);
            var countries = JsonSerializer.Deserialize<List<Country>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new();

            var existingIds = await _context.Countries
                .Select(c => c.CountryID)
                .ToListAsync(cancellationToken);

            var newCountries = countries
                .Where(c => !existingIds.Contains(c.CountryID))
                .ToList();

            if (newCountries.Any())
            {
                await _context.Countries.AddRangeAsync(newCountries, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
