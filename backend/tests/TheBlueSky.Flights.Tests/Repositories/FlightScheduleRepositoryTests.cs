using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories;

namespace TheBlueSky.Flights.Tests.Repositories
{
    [TestFixture]
    public class FlightScheduleRepositoryTests
    {
        private SqliteConnection _connection = null!;
        private DbContextOptions<FlightsDbContext> _options = null!;

        [SetUp]
        public void SetUp()
        {
            // Keep unit tests simple: turn off FK enforcement to avoid seeding Aircraft/Route graph
            _connection = new SqliteConnection("DataSource=:memory:;Foreign Keys=False;");
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

        [Test]
        public async Task GetAll_ReturnsList()
        {
            // Arrange
            using (var seedCtx = new FlightsDbContext(_options))
            {
                seedCtx.FlightSchedules.Add(new FlightSchedule
                {
                    AircraftId = 10,
                    RouteId = 20,
                    FlightNumber = "TB101",
                    FlightName = "Daily",
                    DepartureTime = new TimeOnly(9, 0),
                    ArrivalTime = new TimeOnly(11, 30),
                    BaseFare = 4500m,
                    CheckinBaggageWeightKg = 15,
                    CabinBaggageWeightKg = 7,
                    ValidFrom = DateOnly.FromDateTime(DateTime.Today),
                    ValidUntil = DateOnly.FromDateTime(DateTime.Today.AddDays(30))
                });
                await seedCtx.SaveChangesAsync();
            }

            // Act
            using var ctx = new FlightsDbContext(_options);
            var repo = new FlightScheduleRepository(ctx);
            var all = await repo.GetAllFlightSchedulesAsync();

            // Assert
            Assert.That(all.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNull()
        {
            // Arrange
            using var ctx = new FlightsDbContext(_options);
            var repo = new FlightScheduleRepository(ctx);

            // Act
            var schedule = await repo.GetFlightScheduleByIdAsync(999);

            // Assert
            Assert.That(schedule, Is.Null);
        }

        [Test]
        public async Task Delete_Existing_ReturnsTrue()
        {
            // Arrange
            int id;
            using (var seedCtx = new FlightsDbContext(_options))
            {
                var s = new FlightSchedule
                {
                    AircraftId = 10,
                    RouteId = 20,
                    FlightNumber = "TB202",
                    DepartureTime = new TimeOnly(10, 0),
                    ArrivalTime = new TimeOnly(12, 0),
                    BaseFare = 3500m,
                    CheckinBaggageWeightKg = 15,
                    CabinBaggageWeightKg = 7,
                    ValidFrom = DateOnly.FromDateTime(DateTime.Today),
                    ValidUntil = DateOnly.FromDateTime(DateTime.Today.AddDays(10))
                };
                seedCtx.FlightSchedules.Add(s);
                await seedCtx.SaveChangesAsync();
                id = s.FlightScheduleId;
            }

            using var ctx = new FlightsDbContext(_options);
            var repo = new FlightScheduleRepository(ctx);

            // Act
            var removed = await repo.DeleteFlightScheduleAsync(id);
            var exists = await repo.ExistsAsync(id);

            // Assert
            Assert.That(removed, Is.True);
            Assert.That(exists, Is.False);
        }

        [Test]
        public async Task Update_WhenDeletedInOtherContext_ReturnsFalse()
        {
            // Arrange
            int id;
            using (var seedCtx = new FlightsDbContext(_options))
            {
                var s = new FlightSchedule
                {
                    AircraftId = 10,
                    RouteId = 20,
                    FlightNumber = "TB303",
                    DepartureTime = new TimeOnly(8, 0),
                    ArrivalTime = new TimeOnly(10, 0),
                    BaseFare = 3000m,
                    CheckinBaggageWeightKg = 15,
                    CabinBaggageWeightKg = 7,
                    ValidFrom = DateOnly.FromDateTime(DateTime.Today),
                    ValidUntil = DateOnly.FromDateTime(DateTime.Today.AddDays(20))
                };
                seedCtx.FlightSchedules.Add(s);
                await seedCtx.SaveChangesAsync();
                id = s.FlightScheduleId;
            }

            using var ctx1 = new FlightsDbContext(_options);
            var repo = new FlightScheduleRepository(ctx1);

            var tracked = await ctx1.FlightSchedules.FirstAsync(x => x.FlightScheduleId == id);
            tracked.FlightName = "Updated Name";

            // simulate concurrency delete
            using (var ctx2 = new FlightsDbContext(_options))
            {
                var toDelete = await ctx2.FlightSchedules.FindAsync(id);
                ctx2.Remove(toDelete!);
                await ctx2.SaveChangesAsync();
            }

            // Act
            var ok = await repo.UpdateFlightScheduleAsync(tracked);

            // Assert
            Assert.That(ok, Is.False);
        }
    }
}