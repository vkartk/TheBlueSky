using Microsoft.AspNetCore.Mvc;
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
        private FlightController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            _service = new Mock<IFlightService>();
            _controller = new FlightController(_service.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOk()
        {
            var list = new List<FlightResponse>
            {
                new(1, 10, DateOnly.FromDateTime(DateTime.Today), DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(2), FlightStatus.Scheduled, 100, DateTime.UtcNow)
            };
            _service.Setup(s => s.GetAllFlightsAsync()).ReturnsAsync(list);

            var result = await _controller.GetAllFlights();

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetById_NotFound_Returns404()
        {
            _service.Setup(s => s.GetFlightByIdAsync(99)).ReturnsAsync((FlightResponse?)null);

            var result = await _controller.GetFlightById(99);

            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Create_InvalidModel_Returns400()
        {
            _controller.ModelState.AddModelError("FlightDate", "Required");

            var result = await _controller.CreateFlight(new CreateFlightRequest(1, DateOnly.FromDateTime(DateTime.Today), DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(1), FlightStatus.Scheduled, 100));

            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Create_ValidRequest_Returns201()
        {
            var request = new CreateFlightRequest(10, DateOnly.FromDateTime(DateTime.Today), DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(2), FlightStatus.Scheduled, 100);
            var created = new FlightResponse(1, 10, request.FlightDate, request.DepartureDateTime, request.ArrivalDateTime, FlightStatus.Scheduled, 100, DateTime.UtcNow);

            _service.Setup(s => s.CreateFlightAsync(request)).ReturnsAsync(created);

            var result = await _controller.CreateFlight(request);

            Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());
        }

        [Test]
        public async Task Update_NotFound_Returns404()
        {
            var request = new UpdateFlightRequest(99, DateOnly.FromDateTime(DateTime.Today), DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(1), FlightStatus.Scheduled, 100);
            _service.Setup(s => s.UpdateFlightAsync(request)).ReturnsAsync(false);

            var result = await _controller.UpdateFlight(request);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Update_Found_Returns204()
        {
            var request = new UpdateFlightRequest(1, DateOnly.FromDateTime(DateTime.Today), DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(1), FlightStatus.Scheduled, 100);
            _service.Setup(s => s.UpdateFlightAsync(request)).ReturnsAsync(true);

            var result = await _controller.UpdateFlight(request);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_NotFound_Returns404()
        {
            _service.Setup(s => s.DeleteFlightAsync(99)).ReturnsAsync(false);

            var result = await _controller.DeleteFlightById(99);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Delete_Found_Returns204()
        {
            _service.Setup(s => s.DeleteFlightAsync(1)).ReturnsAsync(true);

            var result = await _controller.DeleteFlightById(1);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }
    }
}
