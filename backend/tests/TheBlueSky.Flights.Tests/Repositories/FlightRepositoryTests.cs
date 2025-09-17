using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TheBlueSky.Flights.Enums;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories;

namespace TheBlueSky.Flights.Tests.Repositories
{
    [TestFixture]
    public class FlightRepositoryTests
    {
        private SqliteConnection _connection = null!;
        private DbContextOptions<FlightsDbContext> _options = null!;

        [SetUp]
        public void SetUp()
        {
            // Disable FK enforcement to avoid needing full FlightSchedule seed
            _connection = new SqliteConnection("DataSource=:memory:;Foreign Keys=False;");
            _connection.Open();

            var services = new ServiceCollection().AddLogging().AddEntityFrameworkSqlite().BuildServiceProvider();

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

        [Test]
        public async Task GetAll_ReturnsList()
        {
            using (var seedCtx = new FlightsDbContext(_options))
            {
                seedCtx.Flights.Add(new Flight
                {
                    FlightScheduleId = 1,
                    FlightDate = DateOnly.FromDateTime(DateTime.Today),
                    DepartureDateTime = DateTimeOffset.UtcNow,
                    ArrivalDateTime = DateTimeOffset.UtcNow.AddHours(2),
                    FlightStatus = FlightStatus.Scheduled,
                    AvailableSeats = 100
                });
                await seedCtx.SaveChangesAsync();
            }

            using var ctx = new FlightsDbContext(_options);
            var repo = new FlightRepository(ctx);
            var all = await repo.GetAllFlightsAsync();

            Assert.That(all.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNull()
        {
            using var ctx = new FlightsDbContext(_options);
            var repo = new FlightRepository(ctx);

            var flight = await repo.GetFlightByIdAsync(999);

            Assert.That(flight, Is.Null);
        }

        [Test]
        public async Task Delete_Existing_ReturnsTrue()
        {
            int id;
            using (var seedCtx = new FlightsDbContext(_options))
            {
                var f = new Flight
                {
                    FlightScheduleId = 1,
                    FlightDate = DateOnly.FromDateTime(DateTime.Today),
                    DepartureDateTime = DateTimeOffset.UtcNow,
                    ArrivalDateTime = DateTimeOffset.UtcNow.AddHours(2),
                    FlightStatus = FlightStatus.Scheduled,
                    AvailableSeats = 100
                };
                seedCtx.Flights.Add(f);
                await seedCtx.SaveChangesAsync();
                id = f.FlightId;
            }

            using var ctx = new FlightsDbContext(_options);
            var repo = new FlightRepository(ctx);

            var removed = await repo.DeleteFlightAsync(id);
            var exists = await repo.ExistsAsync(id);

            Assert.That(removed, Is.True);
            Assert.That(exists, Is.False);
        }

        [Test]
        public async Task Update_WhenDeletedInOtherContext_ReturnsFalse()
        {
            int id;
            using (var seedCtx = new FlightsDbContext(_options))
            {
                var f = new Flight
                {
                    FlightScheduleId = 1,
                    FlightDate = DateOnly.FromDateTime(DateTime.Today),
                    DepartureDateTime = DateTimeOffset.UtcNow,
                    ArrivalDateTime = DateTimeOffset.UtcNow.AddHours(2),
                    FlightStatus = FlightStatus.Scheduled,
                    AvailableSeats = 100
                };
                seedCtx.Flights.Add(f);
                await seedCtx.SaveChangesAsync();
                id = f.FlightId;
            }

            using var ctx1 = new FlightsDbContext(_options);
            var repo = new FlightRepository(ctx1);
            var tracked = await ctx1.Flights.FirstAsync(s => s.FlightId == id);

            using (var ctx2 = new FlightsDbContext(_options))
            {
                var toDelete = await ctx2.Flights.FindAsync(id);
                ctx2.Remove(toDelete!);
                await ctx2.SaveChangesAsync();
            }

            var ok = await repo.UpdateFlightAsync(tracked);

            Assert.That(ok, Is.False);
        }
    }
}
