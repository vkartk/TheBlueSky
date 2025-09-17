using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TheBlueSky.Flights.Controllers;
using TheBlueSky.Flights.DTOs.Requests.FlightSchedule;
using TheBlueSky.Flights.DTOs.Responses.FlightSchedule;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Tests.Controllers
{
    [TestFixture]
    public class FlightScheduleControllerTests
    {
        private Mock<IFlightScheduleService> _service = null!;
        private Mock<ILogger<FlightScheduleController>> _loggerMock = null!;
        private FlightScheduleController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            _service = new Mock<IFlightScheduleService>();
            _loggerMock = new Mock<ILogger<FlightScheduleController>>();
            _controller = new FlightScheduleController(_service.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOk()
        {
            // Arrange
            var list = new List<FlightScheduleResponse>
            {
                new(1, 10, 20, "TB101", "Daily", new TimeOnly(9,0), new TimeOnly(11,30), 4500m, 15, 7,
                    DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(30)), true, DateTime.UtcNow)
            };
            _service.Setup(s => s.GetAllFlightSchedulesAsync()).ReturnsAsync(list);

            // Act
            var result = await _controller.GetAllFlightSchedules();

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetById_NotFound_Returns404()
        {
            // Arrange
            _service.Setup(s => s.GetFlightScheduleByIdAsync(999)).ReturnsAsync((FlightScheduleResponse?)null);

            // Act
            var result = await _controller.GetFlightScheduleById(999);

            // Assert
            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Create_InvalidModel_Returns400()
        {
            // Arrange
            _controller.ModelState.AddModelError("FlightNumber", "Required");

            // Act
            var result = await _controller.CreateFlightSchedule(new CreateFlightScheduleRequest(
                AircraftId: 0, RouteId: 0, FlightNumber: "", FlightName: null,
                DepartureTime: new TimeOnly(0, 0), ArrivalTime: new TimeOnly(0, 0),
                BaseFare: 0m, CheckinBaggageWeightKg: 0, CabinBaggageWeightKg: 0,
                ValidFrom: DateOnly.MinValue, ValidUntil: DateOnly.MinValue));

            // Assert
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Create_ValidRequest_Returns201()
        {
            // Arrange
            var req = new CreateFlightScheduleRequest(
                AircraftId: 10, RouteId: 20, FlightNumber: "TB202", FlightName: "Test",
                DepartureTime: new TimeOnly(10, 0), ArrivalTime: new TimeOnly(12, 0),
                BaseFare: 3500m, CheckinBaggageWeightKg: 15, CabinBaggageWeightKg: 7,
                ValidFrom: DateOnly.FromDateTime(DateTime.Today),
                ValidUntil: DateOnly.FromDateTime(DateTime.Today.AddDays(10))
            );
            var created = new FlightScheduleResponse(
                5, 10, 20, "TB202", "Test", new TimeOnly(10, 0), new TimeOnly(12, 0),
                3500m, 15, 7, req.ValidFrom, req.ValidUntil, true, DateTime.UtcNow);

            _service.Setup(s => s.CreateFlightScheduleAsync(req)).ReturnsAsync(created);

            // Act
            var result = await _controller.CreateFlightSchedule(req);

            // Assert
            Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());
        }

        [Test]
        public async Task Update_InvalidModel_Returns400()
        {
            // Arrange
            _controller.ModelState.AddModelError("FlightNumber", "Required");
            var req = new UpdateFlightScheduleRequest(
                FlightScheduleId: 1, FlightNumber: "", FlightName: "X",
                DepartureTime: new TimeOnly(8, 0), ArrivalTime: new TimeOnly(10, 0),
                BaseFare: 3000m, CheckinBaggageWeightKg: 15, CabinBaggageWeightKg: 7,
                ValidFrom: DateOnly.FromDateTime(DateTime.Today),
                ValidUntil: DateOnly.FromDateTime(DateTime.Today.AddDays(5)),
                IsActive: true
            );

            // Act
            var result = await _controller.UpdateFlightSchedule(req);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Update_NotFound_Returns404()
        {
            // Arrange
            var req = new UpdateFlightScheduleRequest(
                FlightScheduleId: 999, FlightNumber: "TB303", FlightName: "Upd",
                DepartureTime: new TimeOnly(8, 0), ArrivalTime: new TimeOnly(10, 0),
                BaseFare: 3000m, CheckinBaggageWeightKg: 15, CabinBaggageWeightKg: 7,
                ValidFrom: DateOnly.FromDateTime(DateTime.Today),
                ValidUntil: DateOnly.FromDateTime(DateTime.Today.AddDays(5)),
                IsActive: true
            );
            _service.Setup(s => s.UpdateFlightScheduleAsync(req)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateFlightSchedule(req);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Update_Found_Returns204()
        {
            // Arrange
            var req = new UpdateFlightScheduleRequest(
                FlightScheduleId: 1, FlightNumber: "TB404", FlightName: "Ok",
                DepartureTime: new TimeOnly(9, 0), ArrivalTime: new TimeOnly(11, 0),
                BaseFare: 3200m, CheckinBaggageWeightKg: 15, CabinBaggageWeightKg: 7,
                ValidFrom: DateOnly.FromDateTime(DateTime.Today),
                ValidUntil: DateOnly.FromDateTime(DateTime.Today.AddDays(5)),
                IsActive: true
            );
            _service.Setup(s => s.UpdateFlightScheduleAsync(req)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateFlightSchedule(req);

            // Assert
            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_NotFound_Returns404()
        {
            // Arrange
            _service.Setup(s => s.DeleteFlightScheduleAsync(999)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteFlightScheduleById(999);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Delete_Found_Returns204()
        {
            // Arrange
            _service.Setup(s => s.DeleteFlightScheduleAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteFlightScheduleById(1);

            // Assert
            Assert.That(result, Is.TypeOf<NoContentResult>());
        }
    }
}
