using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories;

namespace TheBlueSky.Flights.Tests.Repositories
{
    [TestFixture]
    public class AirportRepositoryTests
    {
        private SqliteConnection _connection = null!;
        private DbContextOptions<FlightsDbContext> _options = null!;

        [SetUp]
        public void SetUp()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var services = new ServiceCollection()
                .AddLogging()
                .AddEntityFrameworkSqlite()
                .BuildServiceProvider();

            _options = new DbContextOptionsBuilder<FlightsDbContext>()
                .UseSqlite(_connection)
                .UseInternalServiceProvider(services)
                .Options;

            using var ctx = new FlightsDbContext(_options);
            ctx.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            _connection.Dispose();
        }

        private void SeedCountry(FlightsDbContext ctx, string countryId = "IN")
        {
            if (!ctx.Countries.Any(c => c.CountryID == countryId))
            {
                ctx.Countries.Add(new Country
                {
                    CountryID = countryId,
                    CountryName = "India",
                    CurrencyCode = "INR"
                });
                ctx.SaveChanges();
            }
        }

        [Test]
        public async Task GetAll_ReturnsList()
        {
            using (var seedCtx = new FlightsDbContext(_options))
            {
                SeedCountry(seedCtx);
                seedCtx.Airports.Add(new Airport
                {
                    AirportCode = "DEL",
                    AirportName = "Indira Gandhi",
                    City = "Delhi",
                    CountryId = "IN"
                });
                await seedCtx.SaveChangesAsync();
            }

            using var ctx = new FlightsDbContext(_options);
            var repo = new AirportRepository(ctx);
            var all = await repo.GetAllAirportsAsync();

            Assert.That(all.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNull()
        {
            using var ctx = new FlightsDbContext(_options);
            var repo = new AirportRepository(ctx);

            var airport = await repo.GetAirportByIdAsync(999);

            Assert.That(airport, Is.Null);
        }

        [Test]
        public async Task Delete_Existing_ReturnsTrue()
        {
            int id;
            using (var seedCtx = new FlightsDbContext(_options))
            {
                SeedCountry(seedCtx);
                var airport = new Airport
                {
                    AirportCode = "DEL",
                    AirportName = "Indira Gandhi",
                    City = "Delhi",
                    CountryId = "IN"
                };
                seedCtx.Airports.Add(airport);
                await seedCtx.SaveChangesAsync();
                id = airport.AirportId;
            }

            using var ctx = new FlightsDbContext(_options);
            var repo = new AirportRepository(ctx);

            var removed = await repo.DeleteAirportAsync(id);
            var exists = await repo.ExistsAsync(id);

            Assert.That(removed, Is.True);
            Assert.That(exists, Is.False);
        }
    }
}
