using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories;

namespace TheBlueSky.Bookings.Tests.Repositories
{
    [TestFixture]
    public class PassengerRepositoryTests
    {
        private DbConnection _connection = null!;
        private DbContextOptions<BookingsDbContext> _options = null!;

        private static DbContextOptions<BookingsDbContext> CreateOptions(DbConnection connection)
        {
            var services = new ServiceCollection()
                .AddLogging()
                .AddEntityFrameworkSqlite()
                .BuildServiceProvider();

            return new DbContextOptionsBuilder<BookingsDbContext>()
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

            using var ctx = new BookingsDbContext(_options);
            ctx.Database.EnsureCreated();

            // Seed minimal data
            ctx.Passengers.AddRange(new[]
            {
                new Passenger
                {
                    PassengerId = 1, ManagedByUserId = 10, FirstName = "A", LastName = "Z",
                    DateOfBirth = new DateTime(1990, 1, 1), Gender = "M",
                    CreatedDate = new DateTime(2023, 1, 1), IsActive = true
                },
                new Passenger
                {
                    PassengerId = 2, ManagedByUserId = 11, FirstName = "B", LastName = "Y",
                    DateOfBirth = new DateTime(1991, 1, 1), Gender = "F",
                    CreatedDate = new DateTime(2024, 1, 1), IsActive = true
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
        public async Task GetAllAsync_ReturnsOrderedByCreatedDateDesc()
        {
            // Arrange
            await using var ctx = new BookingsDbContext(_options);
            var repo = new PassengerRepository(ctx);

            // Act
            var list = await repo.GetAllAsync();

            // Assert
            Assert.That(list, Is.Not.Null);
            Assert.That(new List<Passenger>(list), Has.Count.EqualTo(2));
            var items = new List<Passenger>(list);
            Assert.That(items[0].PassengerId, Is.EqualTo(2));
            Assert.That(items[1].PassengerId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetByIdAsync_NotFound_ReturnsNull()
        {
            // Arrange
            await using var ctx = new BookingsDbContext(_options);
            var repo = new PassengerRepository(ctx);

            // Act
            var entity = await repo.GetByIdAsync(999);

            // Assert
            Assert.That(entity, Is.Null);
        }

        [Test]
        public async Task ExistsAsync_TrueAndFalse_WorkAsExpected()
        {
            // Arrange
            await using var ctx = new BookingsDbContext(_options);
            var repo = new PassengerRepository(ctx);

            // Act & Assert
            Assert.That(await repo.ExistsAsync(1), Is.True);
            Assert.That(await repo.ExistsAsync(999), Is.False);
        }

        [Test]
        public async Task UpdateAsync_EntityMissingDuringSave_ReturnsFalse()
        {
            // Arrange
            await using var ctx1 = new BookingsDbContext(_options);
            await using var ctx2 = new BookingsDbContext(_options);
            var repo = new PassengerRepository(ctx1);

            var entity = await ctx1.Passengers.FindAsync(1);
            Assert.That(entity, Is.Not.Null);
            entity!.FirstName = "Changed";

            // Simulate concurrent delete in another context
            var toRemove = await ctx2.Passengers.FindAsync(1);
            ctx2.Passengers.Remove(toRemove!);
            await ctx2.SaveChangesAsync();

            // Act
            var ok = await repo.UpdateAsync(entity);

            // Assert
            Assert.That(ok, Is.False, "When row is gone, repo should return false (not found).");
        }

        [Test]
        public async Task DeleteAsync_RemovesRow_ReturnsTrue()
        {
            // Arrange
            await using var ctx = new BookingsDbContext(_options);
            var repo = new PassengerRepository(ctx);

            // Act
            var ok = await repo.DeleteAsync(2);

            // Assert
            Assert.That(ok, Is.True);
            Assert.That(await repo.ExistsAsync(2), Is.False);
        }

        [Test]
        public async Task DeleteAsync_NotFound_ReturnsFalse()
        {
            // Arrange
            await using var ctx = new BookingsDbContext(_options);
            var repo = new PassengerRepository(ctx);

            // Act
            var ok = await repo.DeleteAsync(999);

            // Assert
            Assert.That(ok, Is.False);
        }
    }
}