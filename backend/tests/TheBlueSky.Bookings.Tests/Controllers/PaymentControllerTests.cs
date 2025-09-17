using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBlueSky.Bookings.Controllers;
using TheBlueSky.Bookings.DTOs.Requests.Payment;
using TheBlueSky.Bookings.DTOs.Responses.Payment;
using TheBlueSky.Bookings.Enums;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Tests.Controllers
{
    [TestFixture]
    public class PaymentControllerTests
    {
        private Mock<IPaymentService> _service = null!;
        private PaymentController _sut = null!;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _service = new Mock<IPaymentService>(MockBehavior.Strict);
            _sut = new PaymentController(_service.Object);
        }

        [TearDown]
        public void TearDown() => _service.VerifyAll();

        [Test]
        public async Task GetAll_ReturnsOk()
        {
            // Arrange
            _service.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<PaymentResponse>());

            // Act
            var result = await _sut.GetAll();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetById_NotFound_Returns404()
        {
            // Arrange
            _service.Setup(s => s.GetByIdAsync(123)).ReturnsAsync((PaymentResponse?)null);

            // Act
            var result = await _sut.GetById(123);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Create_Returns201_WithRouteValues()
        {
            // Arrange
            var req = new CreatePaymentRequest(7, PaymentMethod.Card, 1200m, null, PaymentStatus.Paid, "TXN123", null, null);
            var created = new PaymentResponse(55, 7, PaymentMethod.Card, 1200m, null, PaymentStatus.Paid, "TXN123", null, null);
            _service.Setup(s => s.CreateAsync(req)).ReturnsAsync(created);

            // Act
            var result = await _sut.Create(req);

            // Assert
            var createdAt = result.Result as CreatedAtActionResult;
            Assert.That(createdAt, Is.Not.Null);
            Assert.That(createdAt!.ActionName, Is.EqualTo(nameof(PaymentController.GetById)));
            Assert.That(createdAt.RouteValues!["id"], Is.EqualTo(55));
            Assert.That(createdAt.Value, Is.EqualTo(created));
        }

        [Test]
        public async Task Update_Found_Returns204()
        {
            // Arrange
            var req = new UpdatePaymentRequest(5, 7, PaymentMethod.Upi, 999m, null, PaymentStatus.Paid, "UPI1", null, null);
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
            var req = new UpdatePaymentRequest(999, 7, PaymentMethod.Upi, 999m, null, PaymentStatus.Paid, "UPI1", null, null);
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
            var result = await _sut.Create(new CreatePaymentRequest(0, PaymentMethod.Card, 1200m, null, PaymentStatus.Paid, "TXN", null, null));

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}