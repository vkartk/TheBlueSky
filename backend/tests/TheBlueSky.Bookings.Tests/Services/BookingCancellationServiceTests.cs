using AutoMapper;
using Moq;
using TheBlueSky.Bookings.DTOs.Requests.BookingCancellation;
using TheBlueSky.Bookings.DTOs.Responses.BookingCancellation;
using TheBlueSky.Bookings.Enums;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories.Interfaces;
using TheBlueSky.Bookings.Services;

namespace TheBlueSky.Bookings.Tests.Services
{
    [TestFixture]
    public class BookingCancellationServiceTests
    {
        private Mock<IBookingCancellationRepository> _repo = null!;
        private Mock<IMapper> _mapper = null!;
        private BookingCancellationService _sut = null!;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _repo = new Mock<IBookingCancellationRepository>(MockBehavior.Strict);
            _mapper = new Mock<IMapper>(MockBehavior.Strict);
            _sut = new BookingCancellationService(_repo.Object, _mapper.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _repo.VerifyAll();
            _mapper.VerifyAll();
        }

        [Test]
        public async Task GetAll_ReturnsList()
        {
            // Arrange
            var entities = new List<BookingCancellation>
            {
                new() { BookingCancellationId = 1, BookingId = 10, RefundAmount = 100m, RefundStatus = RefundStatus.Pending, CancellationDate = new DateTime(2025,1,1) },
                new() { BookingCancellationId = 2, BookingId = 11, RefundAmount = 50m,  RefundStatus = RefundStatus.Processed, CancellationDate = new DateTime(2025,1,2) }
            };

            var dtos = new List<BookingCancellationResponse>
            {
                new(1, 10, new DateTime(2025,1,1), 99, 100m, RefundStatus.Pending, null, "reason1", null),
                new(2, 11, new DateTime(2025,1,2), 98,  50m, RefundStatus.Processed, new DateTime(2025,1,3), "reason2", "note")
            };

            _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
            _mapper.Setup(m => m.Map<IEnumerable<BookingCancellationResponse>>(entities)).Returns(dtos);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Exactly(2).Items);
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNull()
        {
            // Arrange
            _repo.Setup(r => r.GetByIdAsync(123)).ReturnsAsync((BookingCancellation?)null);

            // Act
            var res = await _sut.GetByIdAsync(123);

            // Assert
            Assert.That(res, Is.Null);
        }

        [Test]
        public async Task Create_ReturnsCreatedItem()
        {
            // Arrange
            var req = new CreateBookingCancellationRequest(
                BookingId: 10,
                CancelledByUserId: 77,
                RefundAmount: 120m,
                RefundStatus: RefundStatus.Pending,
                RefundDate: null,
                CancellationReason: "Change of plans",
                AdminNotes: null
            );

            var toAdd = new BookingCancellation
            {
                BookingId = 10,
                CancelledByUserId = 77,
                RefundAmount = 120m,
                RefundStatus = RefundStatus.Pending,
                CancellationDate = new DateTime(2025, 1, 1)
            };

            var added = new BookingCancellation
            {
                BookingCancellationId = 42,
                BookingId = 10,
                CancelledByUserId = 77,
                RefundAmount = 120m,
                RefundStatus = RefundStatus.Pending,
                CancellationDate = new DateTime(2025, 1, 1)
            };

            var dto = new BookingCancellationResponse(
                BookingCancellationId: 42,
                BookingId: 10,
                CancellationDate: new DateTime(2025, 1, 1),
                CancelledByUserId: 77,
                RefundAmount: 120m,
                RefundStatus: RefundStatus.Pending,
                RefundDate: null,
                CancellationReason: "Change of plans",
                AdminNotes: null
            );

            _mapper.Setup(m => m.Map<BookingCancellation>(req)).Returns(toAdd);
            _repo.Setup(r => r.AddAsync(toAdd)).ReturnsAsync(added);
            _mapper.Setup(m => m.Map<BookingCancellationResponse>(added)).Returns(dto);

            // Act
            var created = await _sut.CreateAsync(req);

            // Assert
            Assert.That(created.BookingCancellationId, Is.EqualTo(42));
            Assert.That(created.RefundAmount, Is.EqualTo(120m));
        }

        [Test]
        public async Task Update_Found_ReturnsTrue()
        {
            // Arrange
            var req = new UpdateBookingCancellationRequest(
                BookingCancellationId: 5,
                BookingId: 10,
                CancelledByUserId: 77,
                CancellationDate: new DateTime(2025, 1, 1),
                RefundAmount: 100m,
                RefundStatus: RefundStatus.Processed,
                RefundDate: new DateTime(2025, 1, 2),
                CancellationReason: "reason",
                AdminNotes: "note"
            );
            var entity = new BookingCancellation { BookingCancellationId = 5 };

            _mapper.Setup(m => m.Map<BookingCancellation>(req)).Returns(entity);
            _repo.Setup(r => r.UpdateAsync(entity)).ReturnsAsync(true);

            // Act
            var ok = await _sut.UpdateAsync(req);

            // Assert
            Assert.That(ok, Is.True);
        }

        [Test]
        public async Task Update_NotFound_ReturnsFalse()
        {
            // Arrange
            var req = new UpdateBookingCancellationRequest(
                BookingCancellationId: 999,
                BookingId: 10,
                CancelledByUserId: 77,
                CancellationDate: new DateTime(2025, 1, 1),
                RefundAmount: 100m,
                RefundStatus: RefundStatus.Processed,
                RefundDate: null,
                CancellationReason: null,
                AdminNotes: null
            );
            var entity = new BookingCancellation { BookingCancellationId = 999 };

            _mapper.Setup(m => m.Map<BookingCancellation>(req)).Returns(entity);
            _repo.Setup(r => r.UpdateAsync(entity)).ReturnsAsync(false);

            // Act
            var ok = await _sut.UpdateAsync(req);

            // Assert
            Assert.That(ok, Is.False);
        }

        [Test]
        public async Task Delete_Found_ReturnsTrue()
        {
            // Arrange
            _repo.Setup(r => r.DeleteAsync(10)).ReturnsAsync(true);

            // Act
            var ok = await _sut.DeleteAsync(10);

            // Assert
            Assert.That(ok, Is.True);
        }

        [Test]
        public void Create_WhenRepositoryFails_ThrowsException()
        {
            // Arrange
            var req = new CreateBookingCancellationRequest(
                BookingId: 10,
                CancelledByUserId: 77,
                RefundAmount: 50m,
                RefundStatus: RefundStatus.Pending,
                RefundDate: null,
                CancellationReason: "reason",
                AdminNotes: null
            );

            var mapped = new BookingCancellation();
            _mapper.Setup(m => m.Map<BookingCancellation>(req)).Returns(mapped);
            _repo.Setup(r => r.AddAsync(mapped)).ThrowsAsync(new InvalidOperationException("boom"));

            // Act / Assert
            Assert.That(async () => await _sut.CreateAsync(req), Throws.InvalidOperationException);
        }
    }
}