using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBlueSky.Bookings.Controllers;
using TheBlueSky.Bookings.DTOs.Requests.Booking;
using TheBlueSky.Bookings.DTOs.Responses.Booking;
using TheBlueSky.Bookings.Enums;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Tests.Controllers
{
    [TestFixture]
    public class BookingControllerTests
    {
        private Mock<IBookingService> _service = null!;
        private BookingController _sut = null!;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _service = new Mock<IBookingService>(MockBehavior.Strict);
            _sut = new BookingController(_service.Object);
        }

        [TearDown]
        public void TearDown() => _service.VerifyAll();

        [Test]
        public async Task GetAll_ReturnsOk()
        {
            // Arrange
            _service.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<BookingResponse>());

            // Act
            var result = await _sut.GetAll();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetById_NotFound_Returns404()
        {
            // Arrange
            _service.Setup(s => s.GetByIdAsync(123)).ReturnsAsync((BookingResponse?)null);

            // Act
            var result = await _sut.GetById(123);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Create_Returns201_WithRouteValues()
        {
            // Arrange
            var req = new CreateBookingRequest(
                UserId: 7, FlightId: 100, NumberOfPassengers: 2,
                SubtotalAmount: 1000m, TaxAmount: 100m,
                BookingStatus: BookingStatus.Pending, PaymentStatus: PaymentStatus.Pending
            );

            var created = new BookingResponse(
                BookingId: 55, UserId: 7, FlightId: 100, BookingDate: System.DateTime.UtcNow,
                NumberOfPassengers: 2, SubtotalAmount: 1000m, TaxAmount: 100m, TotalAmount: 1100m,
                BookingStatus: BookingStatus.Pending, PaymentStatus: PaymentStatus.Pending, LastUpdated: System.DateTime.UtcNow
            );

            _service.Setup(s => s.CreateAsync(req)).ReturnsAsync(created);

            // Act
            var result = await _sut.Create(req);

            // Assert
            var createdAt = result.Result as CreatedAtActionResult;
            Assert.That(createdAt, Is.Not.Null);
            Assert.That(createdAt!.ActionName, Is.EqualTo(nameof(BookingController.GetById)));
            Assert.That(createdAt.RouteValues!["id"], Is.EqualTo(55));
            Assert.That(createdAt.Value, Is.EqualTo(created));
        }

        [Test]
        public async Task Update_Found_Returns204()
        {
            // Arrange
            var req = new UpdateBookingRequest(
                BookingId: 5, UserId: 7, FlightId: 100, NumberOfPassengers: 3,
                SubtotalAmount: 1200m, TaxAmount: 120m,
                BookingStatus: BookingStatus.Confirmed, PaymentStatus: PaymentStatus.Pending
            );

            _service.Setup(s => s.UpdateAsync(req)).ReturnsAsync(true);

            // Act
            var result = await _sut.Update(req);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Update_NotFound_Returns404()
        {
            // Arrange
            var req = new UpdateBookingRequest(
                BookingId: 999, UserId: 7, FlightId: 100, NumberOfPassengers: 3,
                SubtotalAmount: 1200m, TaxAmount: 120m,
                BookingStatus: BookingStatus.Confirmed, PaymentStatus: PaymentStatus.Pending
            );

            _service.Setup(s => s.UpdateAsync(req)).ReturnsAsync(false);

            // Act
            var result = await _sut.Update(req);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Delete_Found_Returns204()
        {
            // Arrange
            _service.Setup(s => s.DeleteAsync(10)).ReturnsAsync(true);

            // Act
            var result = await _sut.Delete(10);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_NotFound_Returns404()
        {
            // Arrange
            _service.Setup(s => s.DeleteAsync(10)).ReturnsAsync(false);

            // Act
            var result = await _sut.Delete(10);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Create_InvalidModel_Returns400()
        {
            // Arrange
            _sut.ModelState.AddModelError("UserId", "Required");

            // Act
            var result = await _sut.Create(new CreateBookingRequest(0, 100, 2, 1000m, 100m, BookingStatus.Pending, PaymentStatus.Pending));

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}