
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TheBlueSky.Flights.Controllers;
using TheBlueSky.Flights.DTOs.Requests.Aircraft;
using TheBlueSky.Flights.DTOs.Responses.Aircraft;
using TheBlueSky.Flights.Enums;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Tests.Controllers
{
    [TestFixture]
    public class AircraftControllerTests
    {
        private Mock<IAircraftService> _serviceMock = null!;
        private AircraftController _sut = null!;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _serviceMock = new Mock<IAircraftService>(MockBehavior.Strict);
            _sut = new AircraftController(_serviceMock.Object);
        }

        [Test]
        public async Task GetAllAircrafts_HappyPath_ReturnsOk200()
        {
            // Arrange
            var list = new List<AircraftResponse>
            {
                new(1, 10, "A320", "A320neo", AircraftManufacturer.Airbus, 150, 12, 0, true, System.DateTime.UtcNow)
            };
            _serviceMock.Setup(s => s.GetAllAircraftsAsync()).ReturnsAsync(list);

            // Act
            var result = await _sut.GetAllAircrafts();

            // Assert
            var ok = result.Result as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            Assert.That(ok!.StatusCode, Is.EqualTo(200));
            var payload = ok.Value as IEnumerable<AircraftResponse>;
            Assert.That(payload, Is.Not.Null);
            _serviceMock.Verify(s => s.GetAllAircraftsAsync(), Times.Once);
        }

        [Test]
        public async Task GetAircraftById_NotFound_Returns404()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetAircraftByIdAsync(999)).ReturnsAsync((AircraftResponse?)null);

            // Act
            var result = await _sut.GetAircraftById(999);

            // Assert
            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateAircraft_InvalidModelState_Returns400()
        {
            // Arrange
            _sut.ModelState.AddModelError("AircraftName", "Required");

            var request = new CreateAircraftRequest(
                OwnerUserId: 1, AircraftName: "", AircraftModel: "X",
                Manufacturer: AircraftManufacturer.Airbus, EconomySeats: 1, BusinessSeats: 0, FirstClassSeats: 0
            );

            // Act
            var result = await _sut.CreateAircraft(request);

            // Assert
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task CreateAircraft_Valid_Returns201Created()
        {
            // Arrange
            var request = new CreateAircraftRequest(
                OwnerUserId: 1, AircraftName: "A", AircraftModel: "B",
                Manufacturer: AircraftManufacturer.Airbus, EconomySeats: 1, BusinessSeats: 0, FirstClassSeats: 0
            );

            var created = new AircraftResponse(
                AircraftId: 42, OwnerUserId: 1, AircraftName: "A", AircraftModel: "B",
                Manufacturer: AircraftManufacturer.Airbus, EconomySeats: 1, BusinessSeats: 0, FirstClassSeats: 0,
                IsActive: true, CreatedDate: System.DateTime.UtcNow
            );

            _serviceMock.Setup(s => s.CreateAircraftAsync(request)).ReturnsAsync(created);

            // Act
            var result = await _sut.CreateAircraft(request);

            // Assert
            var createdAt = result.Result as CreatedAtActionResult;
            Assert.That(createdAt, Is.Not.Null);
            Assert.That(createdAt!.StatusCode, Is.EqualTo(201));
            Assert.That(createdAt.Value, Is.EqualTo(created));
            _serviceMock.Verify(s => s.CreateAircraftAsync(request), Times.Once);
        }

        [Test]
        public async Task UpdateAircraft_NotFound_Returns404()
        {
            // Arrange
            var update = new UpdateAircraftRequest(
                AircraftId: 123, AircraftName: "N", AircraftModel: "M",
                Manufacturer: AircraftManufacturer.Boeing, EconomySeats: 10, BusinessSeats: 2, FirstClassSeats: 1, IsActive: true
            );
            _serviceMock.Setup(s => s.UpdateAircraftAsync(update)).ReturnsAsync(false);

            // Act
            var result = await _sut.UpdateAircraft(update);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteAircraftById_NotFound_Returns404()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAircraftAsync(77)).ReturnsAsync(false);

            // Act
            var result = await _sut.DeleteAircraftById(77);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteAircraftById_Success_Returns204()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAircraftAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteAircraftById(1);

            // Assert
            Assert.That(result, Is.TypeOf<NoContentResult>());
        }
    }
}
