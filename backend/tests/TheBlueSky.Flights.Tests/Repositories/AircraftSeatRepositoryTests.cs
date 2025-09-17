using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories;

namespace TheBlueSky.Flights.Tests.Repositories
{
    [TestFixture]
    public class AircraftSeatRepositoryTests
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

        private (int aircraftId, int seatClassId) SeedParents(FlightsDbContext ctx)
        {
            var aircraft = new Aircraft
            {
                OwnerUserId = 1,
                AircraftName = "Test Aircraft",
                AircraftModel = "Model X",
                EconomySeats = 100,
                BusinessSeats = 20,
                FirstClassSeats = 10
            };

            var seatClass = new SeatClass
            {
                ClassName = "Economy",
                PriorityOrder = 1
            };

            ctx.Aircrafts.Add(aircraft);
            ctx.SeatClasses.Add(seatClass);
            ctx.SaveChanges();

            return (aircraft.AircraftId, seatClass.SeatClassId);
        }

        [Test]
        public async Task GetAll_ReturnsList()
        {
            using (var seedCtx = new FlightsDbContext(_options))
            {
                var (aircraftId, seatClassId) = SeedParents(seedCtx);
                seedCtx.AircraftSeats.Add(new AircraftSeat
                {
                    AircraftId = aircraftId,
                    SeatClassId = seatClassId,
                    SeatNumber = "1A",
                    SeatPosition = "Window",
                    AdditionalFare = 100,
                    SeatRow = 1,
                    SeatColumn = 1,
                    IsActive = true
                });
                await seedCtx.SaveChangesAsync();
            }

            using var ctx = new FlightsDbContext(_options);
            var repo = new AircraftSeatRepository(ctx);
            var all = await repo.GetAllAircraftSeatsAsync();

            Assert.That(all.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNull()
        {
            using var ctx = new FlightsDbContext(_options);
            var repo = new AircraftSeatRepository(ctx);

            var seat = await repo.GetAircraftSeatByIdAsync(999);

            Assert.That(seat, Is.Null);
        }

        [Test]
        public async Task Update_WhenDeletedInOtherContext_ReturnsFalse()
        {
            int id;
            using (var seedCtx = new FlightsDbContext(_options))
            {
                var (aircraftId, seatClassId) = SeedParents(seedCtx);
                var seat = new AircraftSeat
                {
                    AircraftId = aircraftId,
                    SeatClassId = seatClassId,
                    SeatNumber = "9C",
                    SeatPosition = "Aisle",
                    AdditionalFare = 25,
                    SeatRow = 9,
                    SeatColumn = 3,
                    IsActive = true
                };
                seedCtx.AircraftSeats.Add(seat);
                await seedCtx.SaveChangesAsync();
                id = seat.AircraftSeatId;
            }

            using var ctx1 = new FlightsDbContext(_options);
            var repo = new AircraftSeatRepository(ctx1);

            var tracked = await ctx1.AircraftSeats.FirstAsync(s => s.AircraftSeatId == id);
            tracked.SeatNumber = "9D";

            using (var ctx2 = new FlightsDbContext(_options))
            {
                var seatToDelete = await ctx2.AircraftSeats.FindAsync(id);
                ctx2.Remove(seatToDelete!);
                await ctx2.SaveChangesAsync();
            }

            var ok = await repo.UpdateAircraftSeatAsync(tracked);

            Assert.That(ok, Is.False);
        }

        [Test]
        public async Task Delete_Existing_ReturnsTrue()
        {
            int id;
            using (var seedCtx = new FlightsDbContext(_options))
            {
                var (aircraftId, seatClassId) = SeedParents(seedCtx);
                var seat = new AircraftSeat
                {
                    AircraftId = aircraftId,
                    SeatClassId = seatClassId,
                    SeatNumber = "3A",
                    SeatPosition = "Window",
                    AdditionalFare = 10,
                    SeatRow = 3,
                    SeatColumn = 1,
                    IsActive = true
                };
                seedCtx.AircraftSeats.Add(seat);
                await seedCtx.SaveChangesAsync();
                id = seat.AircraftSeatId;
            }

            using var ctx = new FlightsDbContext(_options);
            var repo = new AircraftSeatRepository(ctx);

            var removed = await repo.DeleteAircraftSeatAsync(id);
            var exists = await repo.ExistsAsync(id);

            Assert.That(removed, Is.True);
            Assert.That(exists, Is.False);
        }
    }
}
