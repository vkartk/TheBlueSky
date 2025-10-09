using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TheBlueSky.Flights.Controllers;
using TheBlueSky.Flights.DTOs.Requests.Flight;
using TheBlueSky.Flights.DTOs.Responses.Flight;
using TheBlueSky.Flights.Enums;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Tests.Controllers
{
    [TestFixture]
    public class FlightControllerTests
    {
        private Mock<IFlightService> _service = null!;
        private Mock<ILogger<FlightController>> _loggerMock = null!;
        private FlightController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            _service = new Mock<IFlightService>();
            _loggerMock = new Mock<ILogger<FlightController>>();
            _controller = new FlightController(_service.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAllFlights_WhenCalled_ShouldReturnOk()
        {
            var list = new List<FlightResponse>
            {
                new(1, 10, DateOnly.FromDateTime(DateTime.Today), DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(2), FlightStatus.Scheduled, 100,5000, DateTime.UtcNow)
            };
            _service.Setup(s => s.GetAllFlightsAsync()).ReturnsAsync(list);

            var result = await _controller.GetAllFlights();

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetFlightById_WhenIdIsUnknown_ShouldReturnNotFound()
        {
            _service.Setup(s => s.GetFlightByIdAsync(99)).ReturnsAsync((FlightDetailsResponse?)null);

            var result = await _controller.GetFlightById(99);

            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateFlight_WhenModelIsInvalid_ShouldReturnBadRequest()
        {
            _controller.ModelState.AddModelError("FlightDate", "Required");

            var result = await _controller.CreateFlight(new CreateFlightRequest(1, DateOnly.FromDateTime(DateTime.Today), DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(1), FlightStatus.Scheduled, 100));

            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task CreateFlight_WhenRequestIsValid_ShouldReturnCreated()
        {
            var request = new CreateFlightRequest(10, DateOnly.FromDateTime(DateTime.Today), DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(2), FlightStatus.Scheduled, 100);
            var created = new FlightResponse(1, 10, request.FlightDate, request.DepartureDateTime, request.ArrivalDateTime, FlightStatus.Scheduled, 100,5000, DateTime.UtcNow);

            _service.Setup(s => s.CreateFlightAsync(request)).ReturnsAsync(created);

            var result = await _controller.CreateFlight(request);

            Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());
        }

        [Test]
        public async Task UpdateFlight_WhenIdIsUnknown_ShouldReturnNotFound()
        {
            var request = new UpdateFlightRequest(99, DateOnly.FromDateTime(DateTime.Today), DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(1), FlightStatus.Scheduled, 100);
            _service.Setup(s => s.UpdateFlightAsync(request)).ReturnsAsync(false);

            var result = await _controller.UpdateFlight(request.FlightId,request);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task UpdateFlight_WhenIdIsValid_ShouldReturnNoContent()
        {
            var request = new UpdateFlightRequest(1, DateOnly.FromDateTime(DateTime.Today), DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(1), FlightStatus.Scheduled, 100);
            _service.Setup(s => s.UpdateFlightAsync(request)).ReturnsAsync(true);

            var result = await _controller.UpdateFlight(request.FlightId, request);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteFlight_WhenIdIsUnknown_ShouldReturnNotFound()
        {
            _service.Setup(s => s.DeleteFlightAsync(99)).ReturnsAsync(false);

            var result = await _controller.DeleteFlightById(99);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteFlight_WhenIdIsValid_ShouldReturnNoContent()
        {
            _service.Setup(s => s.DeleteFlightAsync(1)).ReturnsAsync(true);

            var result = await _controller.DeleteFlightById(1);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }
    }
}
