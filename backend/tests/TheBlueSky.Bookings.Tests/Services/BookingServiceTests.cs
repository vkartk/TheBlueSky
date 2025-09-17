using AutoMapper;
using Moq;
using TheBlueSky.Bookings.DTOs.Requests.Booking;
using TheBlueSky.Bookings.DTOs.Responses.Booking;
using TheBlueSky.Bookings.Enums;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories.Interfaces;
using TheBlueSky.Bookings.Services;

namespace TheBlueSky.Bookings.Tests.Services
{
    [TestFixture]
    public class BookingServiceTests
    {
        private Mock<IBookingRepository> _repo = null!;
        private Mock<IMapper> _mapper = null!;
        private BookingService _sut = null!;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _repo = new Mock<IBookingRepository>(MockBehavior.Strict);
            _mapper = new Mock<IMapper>(MockBehavior.Strict);
            _sut = new BookingService(_repo.Object, _mapper.Object);
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
            var entities = new List<Booking>
            {
                new() { BookingId = 1, UserId = 7, FlightId = 100, NumberOfPassengers = 2, SubtotalAmount = 1000m, TaxAmount = 100m, BookingDate = new DateTime(2025,1,1) },
                new() { BookingId = 2, UserId = 8, FlightId = 101, NumberOfPassengers = 1, SubtotalAmount = 800m,  TaxAmount =  80m, BookingDate = new DateTime(2025,1,2) }
            };

            var dtos = new List<BookingResponse>
            {
                new(
                    BookingId: 1, UserId: 7, FlightId: 100, BookingDate: new DateTime(2025,1,1),
                    NumberOfPassengers: 2, SubtotalAmount: 1000m, TaxAmount: 100m, TotalAmount: 1100m,
                    BookingStatus: BookingStatus.Pending, PaymentStatus: PaymentStatus.Pending, LastUpdated: new DateTime(2025,1,1)
                ),
                new(
                    BookingId: 2, UserId: 8, FlightId: 101, BookingDate: new DateTime(2025,1,2),
                    NumberOfPassengers: 1, SubtotalAmount: 800m, TaxAmount: 80m, TotalAmount: 880m,
                    BookingStatus: BookingStatus.Pending, PaymentStatus: PaymentStatus.Pending, LastUpdated: new DateTime(2025,1,2)
                )
            };

            _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
            _mapper.Setup(m => m.Map<IEnumerable<BookingResponse>>(entities)).Returns(dtos);

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
            _repo.Setup(r => r.GetByIdAsync(123)).ReturnsAsync((Booking?)null);

            // Act
            var res = await _sut.GetByIdAsync(123);

            // Assert
            Assert.That(res, Is.Null);
        }

        [Test]
        public async Task Create_ReturnsCreatedItem()
        {
            // Arrange
            var req = new CreateBookingRequest(
                UserId: 7,
                FlightId: 100,
                NumberOfPassengers: 2,
                SubtotalAmount: 1000m,
                TaxAmount: 100m,
                BookingStatus: BookingStatus.Pending,
                PaymentStatus: PaymentStatus.Pending
            );

            var toAdd = new Booking
            {
                UserId = 7,
                FlightId = 100,
                NumberOfPassengers = 2,
                SubtotalAmount = 1000m,
                TaxAmount = 100m,
                BookingDate = new DateTime(2025, 1, 1),
                LastUpdated = new DateTime(2025, 1, 1),
                BookingStatus = BookingStatus.Pending,
                PaymentStatus = PaymentStatus.Pending
            };

            var added = new Booking
            {
                BookingId = 42,
                UserId = 7,
                FlightId = 100,
                NumberOfPassengers = 2,
                SubtotalAmount = 1000m,
                TaxAmount = 100m,
                BookingDate = new DateTime(2025, 1, 1),
                LastUpdated = new DateTime(2025, 1, 1),
                BookingStatus = BookingStatus.Pending,
                PaymentStatus = PaymentStatus.Pending
            };

            var dto = new BookingResponse(
                BookingId: 42, UserId: 7, FlightId: 100, BookingDate: new DateTime(2025, 1, 1),
                NumberOfPassengers: 2, SubtotalAmount: 1000m, TaxAmount: 100m, TotalAmount: 1100m,
                BookingStatus: BookingStatus.Pending, PaymentStatus: PaymentStatus.Pending, LastUpdated: new DateTime(2025, 1, 1)
            );

            _mapper.Setup(m => m.Map<Booking>(req)).Returns(toAdd);
            _repo.Setup(r => r.AddAsync(toAdd)).ReturnsAsync(added);
            _mapper.Setup(m => m.Map<BookingResponse>(added)).Returns(dto);

            // Act
            var created = await _sut.CreateAsync(req);

            // Assert
            Assert.That(created.BookingId, Is.EqualTo(42));
            Assert.That(created.TotalAmount, Is.EqualTo(1100m));
        }

        [Test]
        public async Task Update_Found_ReturnsTrue()
        {
            // Arrange
            var req = new UpdateBookingRequest(
                BookingId: 5, UserId: 7, FlightId: 100, NumberOfPassengers: 3,
                SubtotalAmount: 1200m, TaxAmount: 120m,
                BookingStatus: BookingStatus.Confirmed, PaymentStatus: PaymentStatus.Pending
            );

            var existing = new Booking { BookingId = 5 };

            _repo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(existing);
            _mapper.Setup(m => m.Map(req, existing)).Returns(existing);
            _repo.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(true);

            // Act
            var ok = await _sut.UpdateAsync(req);

            // Assert
            Assert.That(ok, Is.True);
        }

        [Test]
        public async Task Update_NotFound_ReturnsFalse()
        {
            // Arrange
            var req = new UpdateBookingRequest(
                BookingId: 999, UserId: 7, FlightId: 100, NumberOfPassengers: 3,
                SubtotalAmount: 1200m, TaxAmount: 120m,
                BookingStatus: BookingStatus.Confirmed, PaymentStatus: PaymentStatus.Pending
            );

            _repo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Booking?)null);

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
            var req = new CreateBookingRequest(
                UserId: 7, FlightId: 100, NumberOfPassengers: 2,
                SubtotalAmount: 1000m, TaxAmount: 100m,
                BookingStatus: BookingStatus.Pending, PaymentStatus: PaymentStatus.Pending
            );

            var mapped = new Booking();
            _mapper.Setup(m => m.Map<Booking>(req)).Returns(mapped);
            _repo.Setup(r => r.AddAsync(mapped)).ThrowsAsync(new InvalidOperationException("boom"));

            // Act / Assert
            Assert.That(async () => await _sut.CreateAsync(req), Throws.InvalidOperationException);
        }
    }
}