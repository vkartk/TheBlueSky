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
        public async Task GetAll_WhenCalled_ShouldReturnOkResult()
        {
            // Arrange
            _service.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<BookingCancellationResponse>());

            // Act
            var result = await _sut.GetAll();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetById_WhenEntityDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            _service.Setup(s => s.GetByIdAsync(123)).ReturnsAsync((BookingCancellationResponse?)null);

            // Act
            var result = await _sut.GetById(123);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Create_WhenModelIsValid_ShouldReturnCreatedResultWithRouteValues()
        {
            // Arrange
            var req = new CreateBookingCancellationRequest
            {
                BookingId = 10,
                CancelledByUserId = "77",
                RefundAmount = 120m,
                RefundStatus = RefundStatus.Pending,
                RefundDate = null,
                CancellationReason = "reason",
                AdminNotes = null
            };

            var created = new BookingCancellationResponse(55, 10, System.DateTime.UtcNow, "77", 120m, RefundStatus.Pending, null, "reason", null);

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
        public async Task Update_WhenEntityExists_ShouldReturnNoContent()
        {
            // Arrange
            var req = new UpdateBookingCancellationRequest
            {
                BookingCancellationId = 5,
                BookingId = 10,
                CancelledByUserId = "77",
                CancellationDate = DateTime.UtcNow,
                RefundAmount = 100m,
                RefundStatus = RefundStatus.Processed,
                RefundDate = DateTime.UtcNow,
                CancellationReason = "r",
                AdminNotes = "n"
            };
            _service.Setup(s => s.UpdateAsync(req)).ReturnsAsync(true);

            // Act
            var result = await _sut.Update(req);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Update_WhenEntityDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var req = new UpdateBookingCancellationRequest
            {
                BookingCancellationId = 999,
                BookingId = 10,
                CancelledByUserId = "77",
                CancellationDate = DateTime.UtcNow,
                RefundAmount = 100m,
                RefundStatus = RefundStatus.Processed,
                RefundDate = null,
                CancellationReason = null,
                AdminNotes = null
            };

            _service.Setup(s => s.UpdateAsync(req)).ReturnsAsync(false);

            // Act
            var result = await _sut.Update(req);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Delete_WhenEntityExists_ShouldReturnNoContent()
        {
            // Arrange
            _service.Setup(s => s.DeleteAsync(10)).ReturnsAsync(true);

            // Act
            var result = await _sut.Delete(10);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_WhenEntityDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            _service.Setup(s => s.DeleteAsync(10)).ReturnsAsync(false);

            // Act
            var result = await _sut.Delete(10);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Create_WhenModelIsInvalid_ShouldReturnBadRequest()
        {
            // Arrange
            _sut.ModelState.AddModelError("BookingId", "Required");

            // Act
            var result = await _sut.Create(new CreateBookingCancellationRequest{ BookingId = 0, CancelledByUserId = "77", RefundAmount = 100m, RefundStatus = RefundStatus.Pending, RefundDate = null, CancellationReason = null, AdminNotes = null });

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}
