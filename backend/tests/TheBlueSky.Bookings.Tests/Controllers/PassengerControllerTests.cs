using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
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
        private Mock<ILogger<PassengerController>> _logger = null!;
        private PassengerController _sut = null!;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _service = new Mock<IPassengerService>(MockBehavior.Strict);
            _logger = new Mock<ILogger<PassengerController>>(MockBehavior.Loose);
            _sut = new PassengerController(_service.Object, _logger.Object);
        }

        [TearDown]
        public void TearDown() => _service.VerifyAll();

        [Test]
        public async Task GetAll_WhenCalled_ShouldReturnOkResult()
        {
            // Arrange
            _service.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<PassengerResponse>());

            // Act
            var result = await _sut.GetAll();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetById_WhenEntityDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            _service.Setup(s => s.GetByIdAsync(123)).ReturnsAsync((PassengerResponse?)null);

            // Act
            var result = await _sut.GetById(123);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Create_WhenModelIsValid_ShouldReturnCreatedAtActionWithPassengerId()
        {
            // Arrange
            var req = new CreatePassengerRequest
            {
                ManagedByUserId = "1",
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = DateTime.UtcNow.Date,
                Gender = null,
                PassportNumber = null,
                NationalityCountryId = null,
                RelationshipToManager = null
            };


            var created = new PassengerResponse(
                PassengerId: 55,
                ManagedByUserId: "1",
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
        public async Task Update_WhenEntityExists_ShouldReturnNoContent()
        {
            // Arrange
            var req = new UpdatePassengerRequest
            {
                PassengerId = 5,
                ManagedByUserId = "1",
                FirstName = "J",
                LastName = "D",
                DateOfBirth = DateTime.UtcNow.Date,
                Gender = null,
                PassportNumber = null,
                NationalityCountryId = null,
                RelationshipToManager = null,
                IsActive = true
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
            var req = new UpdatePassengerRequest{ PassengerId = 999, ManagedByUserId = "1", FirstName = "J", LastName = "D", DateOfBirth = DateTime.UtcNow.Date, Gender = null, PassportNumber = null, NationalityCountryId = null, RelationshipToManager = null, IsActive = true };
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
            _sut.ModelState.AddModelError("FirstName", "Required");

            // Act
            var result = await _sut.Create(new CreatePassengerRequest{ ManagedByUserId = "1", FirstName = "", LastName = "Doe", DateOfBirth = DateTime.UtcNow.Date, Gender = null, PassportNumber = null, NationalityCountryId = null, RelationshipToManager = null });

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}