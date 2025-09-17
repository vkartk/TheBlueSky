using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TheBlueSky.Bookings.Controllers;
using TheBlueSky.Bookings.DTOs.Requests.BookingCancellation;
using TheBlueSky.Bookings.DTOs.Responses.BookingCancellation;
using TheBlueSky.Bookings.Enums;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Tests.Controllers
{
    [TestFixture]
    public class BookingCancellationControllerTests
    {
        private Mock<IBookingCancellationService> _service = null!;
        private Mock<ILogger<BookingCancellationController>> _logger = null!;
        private BookingCancellationController _sut = null!;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _service = new Mock<IBookingCancellationService>(MockBehavior.Strict);
            _logger = new Mock<ILogger<BookingCancellationController>>(MockBehavior.Loose);
            _sut = new BookingCancellationController(_service.Object, _logger.Object);
        }

        [TearDown]
        public void TearDown() => _service.VerifyAll();

        [Test]
        public async Task GetAll_ReturnsOk()
        {
            // Arrange
            _service.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<BookingCancellationResponse>());

            // Act
            var result = await _sut.GetAll();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetById_NotFound_Returns404()
        {
            // Arrange
            _service.Setup(s => s.GetByIdAsync(123)).ReturnsAsync((BookingCancellationResponse?)null);

            // Act
            var result = await _sut.GetById(123);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Create_Returns201_WithRouteValues()
        {
            // Arrange
            var req = new CreateBookingCancellationRequest(10, 77, 120m, RefundStatus.Pending, null, "reason", null);
            var created = new BookingCancellationResponse(55, 10, System.DateTime.UtcNow, 77, 120m, RefundStatus.Pending, null, "reason", null);

            _service.Setup(s => s.CreateAsync(req)).ReturnsAsync(created);

            // Act
            var result = await _sut.Create(req);

            // Assert
            var createdAt = result.Result as CreatedAtActionResult;
            Assert.That(createdAt, Is.Not.Null);
            Assert.That(createdAt!.ActionName, Is.EqualTo(nameof(BookingCancellationController.GetById)));
            Assert.That(createdAt.RouteValues!["id"], Is.EqualTo(55));
            Assert.That(createdAt.Value, Is.EqualTo(created));
        }

        [Test]
        public async Task Update_Found_Returns204()
        {
            // Arrange
            var req = new UpdateBookingCancellationRequest(5, 10, 77, System.DateTime.UtcNow, 100m, RefundStatus.Processed, System.DateTime.UtcNow, "r", "n");
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
            var req = new UpdateBookingCancellationRequest(999, 10, 77, System.DateTime.UtcNow, 100m, RefundStatus.Processed, null, null, null);
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
            _sut.ModelState.AddModelError("BookingId", "Required");

            // Act
            var result = await _sut.Create(new CreateBookingCancellationRequest(0, 77, 100m, RefundStatus.Pending, null, null, null));

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}
