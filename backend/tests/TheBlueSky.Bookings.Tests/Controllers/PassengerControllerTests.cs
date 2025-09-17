using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBlueSky.Bookings.Controllers;
using TheBlueSky.Bookings.DTOs.Requests.Passenger;
using TheBlueSky.Bookings.DTOs.Responses.Passenger;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Tests.Controllers
{
    [TestFixture]
    public class PassengerControllerTests
    {
        private Mock<IPassengerService> _service = null!;
        private PassengerController _sut = null!;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _service = new Mock<IPassengerService>(MockBehavior.Strict);
            _sut = new PassengerController(_service.Object);
        }

        [TearDown]
        public void TearDown() => _service.VerifyAll();

        [Test]
        public async Task GetAll_ReturnsOk()
        {
            // Arrange
            _service.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<PassengerResponse>());

            // Act
            var result = await _sut.GetAll();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetById_NotFound_Returns404()
        {
            // Arrange
            _service.Setup(s => s.GetByIdAsync(123)).ReturnsAsync((PassengerResponse?)null);

            // Act
            var result = await _sut.GetById(123);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Create_Returns201_WithRouteValues()
        {
            // Arrange
            var req = new CreatePassengerRequest(1, "John", "Doe", System.DateTime.UtcNow.Date, null, null, null, null);

            var created = new PassengerResponse(
                PassengerId: 55,
                ManagedByUserId: 1,
                FirstName: "John",
                LastName: "Doe",
                DateOfBirth: System.DateTime.UtcNow.Date,
                Gender: null,
                PassportNumber: null,
                NationalityCountryId: null,
                RelationshipToManager: null,
                CreatedDate: System.DateTime.UtcNow,
                IsActive: true
            );

            _service.Setup(s => s.CreateAsync(req)).ReturnsAsync(created);

            // Act
            var result = await _sut.Create(req);

            // Assert
            var createdAt = result.Result as CreatedAtActionResult;
            Assert.That(createdAt, Is.Not.Null);
            Assert.That(createdAt!.ActionName, Is.EqualTo(nameof(PassengerController.GetById)));
            Assert.That(createdAt.RouteValues!["id"], Is.EqualTo(55));
            Assert.That(createdAt.Value, Is.EqualTo(created));
        }

        [Test]
        public async Task Update_Found_Returns204()
        {
            // Arrange
            var req = new UpdatePassengerRequest(5, 1, "J", "D", System.DateTime.UtcNow.Date, null, null, null, null, true);
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
            var req = new UpdatePassengerRequest(999, 1, "J", "D", System.DateTime.UtcNow.Date, null, null, null, null, true);
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
            _sut.ModelState.AddModelError("FirstName", "Required");

            // Act
            var result = await _sut.Create(new CreatePassengerRequest(1, "", "Doe", System.DateTime.UtcNow.Date, null, null, null, null));

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}