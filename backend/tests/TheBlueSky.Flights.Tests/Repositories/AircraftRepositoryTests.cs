using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
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
    public class AircraftRepositoryTests
    {
        private DbConnection _connection = null!;
        private DbContextOptions<FlightsDbContext> _options = null!;

        private static DbContextOptions<FlightsDbContext> CreateOptions(DbConnection connection)
        {
            var services = new ServiceCollection()
                .AddLogging()                
                .AddEntityFrameworkSqlite()  
                .BuildServiceProvider();

            return new DbContextOptionsBuilder<FlightsDbContext>()
                .UseSqlite(connection)
                .UseInternalServiceProvider(services)
                .Options;
        }

        [SetUp]
        public void SetUp()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = CreateOptions(_connection);

            using var ctx = new FlightsDbContext(_options);
            ctx.Database.EnsureCreated();

            // Seed minimal data
            ctx.Aircrafts.AddRange(new[]
            {
                new Aircraft
                {
                    AircraftId = 1, OwnerUserId = 10, AircraftName = "A320",
                    AircraftModel = "A320neo", Manufacturer = AircraftManufacturer.Airbus,
                    EconomySeats = 150, BusinessSeats = 12, FirstClassSeats = 0, IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new Aircraft
                {
                    AircraftId = 2, OwnerUserId = 11, AircraftName = "737",
                    AircraftModel = "737-800", Manufacturer = AircraftManufacturer.Boeing,
                    EconomySeats = 160, BusinessSeats = 8, FirstClassSeats = 0, IsActive = true,
                    CreatedDate = DateTime.UtcNow
                }
            });
            ctx.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _connection.Dispose();
        }

        [Test]
        public async Task GetAllAircraftsAsync_ReturnsSeededRows()
        {
            // Arrange
            await using var ctx = new FlightsDbContext(_options);
            var repo = new AircraftRepository(ctx);

            // Act
            var list = await repo.GetAllAircraftsAsync();

            // Assert
            Assert.That(list, Is.Not.Null);
            Assert.That(new List<Aircraft>(list), Has.Count.EqualTo(2));
        }

        [Test]
        public async Task GetAircraftByIdAsync_NotFound_ReturnsNull()
        {
            // Arrange
            await using var ctx = new FlightsDbContext(_options);
            var repo = new AircraftRepository(ctx);

            // Act
            var entity = await repo.GetAircraftByIdAsync(999);

            // Assert
            Assert.That(entity, Is.Null);
        }

        [Test]
        public async Task ExistsAsync_TrueAndFalse_WorkAsExpected()
        {
            // Arrange
            await using var ctx = new FlightsDbContext(_options);
            var repo = new AircraftRepository(ctx);

            // Act & Assert
            Assert.That(await repo.ExistsAsync(1), Is.True);
            Assert.That(await repo.ExistsAsync(999), Is.False);
        }

        [Test]
        public async Task UpdateAircraftAsync_EntityMissingDuringSave_ReturnsFalse()
        {
            // Arrange
            await using var ctx1 = new FlightsDbContext(_options);
            await using var ctx2 = new FlightsDbContext(_options);
            var repo = new AircraftRepository(ctx1);

            var entity = await ctx1.Aircrafts.FindAsync(1);
            Assert.That(entity, Is.Not.Null);

            // Simulate concurrent delete in another context
            var victim = await ctx2.Aircrafts.FindAsync(1);
            ctx2.Aircrafts.Remove(victim!);
            await ctx2.SaveChangesAsync();

            // Act
            var ok = await repo.UpdateAircraftAsync(entity!);

            // Assert
            Assert.That(ok, Is.False, "When row is gone, repo should return false (not found).");
        }

        [Test]
        public async Task DeleteAircraftAsync_RemovesRow_ReturnsTrue()
        {
            // Arrange
            await using var ctx = new FlightsDbContext(_options);
            var repo = new AircraftRepository(ctx);

            // Act
            var ok = await repo.DeleteAircraftAsync(2);

            // Assert
            Assert.That(ok, Is.True);
            Assert.That(await repo.ExistsAsync(2), Is.False);
        }

        [Test]
        public async Task DeleteAircraftAsync_NotFound_ReturnsFalse()
        {
            // Arrange
            await using var ctx = new FlightsDbContext(_options);
            var repo = new AircraftRepository(ctx);

            // Act
            var ok = await repo.DeleteAircraftAsync(999);

            // Assert
            Assert.That(ok, Is.False);
        }
    }
}
