using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TheBlueSky.Bookings.Enums;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories;

namespace TheBlueSky.Bookings.Tests.Repositories
{
    [TestFixture]
    public class BookingCancellationRepositoryTests
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

            // Seed parent bookings (FK)
            ctx.Bookings.AddRange(new[]
            {
                new Booking
                {
                    BookingId = 10, UserId = 100, FlightId = 200,
                    BookingDate = DateTime.UtcNow.AddDays(-2),
                    NumberOfPassengers = 2, SubtotalAmount = 1000m, TaxAmount = 100m,
                    BookingStatus = BookingStatus.Pending, PaymentStatus = PaymentStatus.Pending,
                    LastUpdated = DateTime.UtcNow.AddDays(-2)
                },
                new Booking
                {
                    BookingId = 11, UserId = 101, FlightId = 201,
                    BookingDate = DateTime.UtcNow.AddDays(-1),
                    NumberOfPassengers = 1, SubtotalAmount = 800m, TaxAmount = 80m,
                    BookingStatus = BookingStatus.Confirmed, PaymentStatus = PaymentStatus.Pending,
                    LastUpdated = DateTime.UtcNow.AddDays(-1)
                }
            });

            // Seed booking cancellations
            ctx.BookingCancellations.AddRange(new[]
            {
                new BookingCancellation
                {
                    BookingCancellationId = 1, BookingId = 10, CancellationDate = DateTime.UtcNow.AddDays(-1),
                    CancelledByUserId = 77, RefundAmount = 100m, RefundStatus = RefundStatus.Pending
                },
                new BookingCancellation
                {
                    BookingCancellationId = 2, BookingId = 11, CancellationDate = DateTime.UtcNow,
                    CancelledByUserId = 78, RefundAmount = 50m, RefundStatus = RefundStatus.Processed, RefundDate = DateTime.UtcNow
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
        public async Task GetAllAsync_ReturnsSeededRows()
        {
            // Arrange
            await using var ctx = new BookingsDbContext(_options);
            var repo = new BookingCancellationRepository(ctx);

            // Act
            var list = await repo.GetAllAsync();

            // Assert
            Assert.That(list, Is.Not.Null);
            Assert.That(new List<BookingCancellation>(list), Has.Count.EqualTo(2));
        }

        [Test]
        public async Task GetByIdAsync_NotFound_ReturnsNull()
        {
            // Arrange
            await using var ctx = new BookingsDbContext(_options);
            var repo = new BookingCancellationRepository(ctx);

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
            var repo = new BookingCancellationRepository(ctx);

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
            var repo = new BookingCancellationRepository(ctx1);

            var entity = await ctx1.BookingCancellations.FindAsync(1);
            Assert.That(entity, Is.Not.Null);
            entity!.RefundAmount = 123m;

            // Simulate concurrent delete in another context
            var toRemove = await ctx2.BookingCancellations.FindAsync(1);
            ctx2.BookingCancellations.Remove(toRemove!);
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
            var repo = new BookingCancellationRepository(ctx);

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
            var repo = new BookingCancellationRepository(ctx);

            // Act
            var ok = await repo.DeleteAsync(999);

            // Assert
            Assert.That(ok, Is.False);
        }
    }
}