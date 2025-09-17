using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TheBlueSky.Flights.Controllers;
using TheBlueSky.Flights.DTOs.Requests.Airport;
using TheBlueSky.Flights.DTOs.Responses.Airport;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Tests.Controllers
{
    [TestFixture]
    public class AirportsControllerTests
    {
        private Mock<IAirportService> _service = null!;
        private Mock<ILogger<AirportsController>> _loggerMock = null!;
        private AirportsController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            _service = new Mock<IAirportService>();
            _loggerMock = new Mock<ILogger<AirportsController>>();
            _controller = new AirportsController(_service.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOk()
        {
            var list = new List<AirportResponse> { new(1, "DEL", "Indira Gandhi", "Delhi", "IN", true) };
            _service.Setup(s => s.GetAllAirportsAsync()).ReturnsAsync(list);

            var result = await _controller.GetAllAirports();

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetById_NotFound_Returns404()
        {
            _service.Setup(s => s.GetAirportByIdAsync(99)).ReturnsAsync((AirportResponse?)null);

            var result = await _controller.GetAirportById(99);

            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Create_InvalidModel_Returns400()
        {
            _controller.ModelState.AddModelError("AirportCode", "Required");

            var result = await _controller.CreateAirport(new CreateAirportRequest("", "", "", ""));

            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Create_ValidRequest_Returns201()
        {
            var request = new CreateAirportRequest("DEL", "Indira Gandhi", "Delhi", "IN");
            var created = new AirportResponse(1, "DEL", "Indira Gandhi", "Delhi", "IN", true);

            _service.Setup(s => s.CreateAirportAsync(request)).ReturnsAsync(created);

            var result = await _controller.CreateAirport(request);

            Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());
        }

        [Test]
        public async Task Update_NotFound_Returns404()
        {
            var request = new UpdateAirportRequest(99, "DEL", "Indira Gandhi", "Delhi", "IN", true);
            _service.Setup(s => s.UpdateAirportAsync(request)).ReturnsAsync(false);

            var result = await _controller.UpdateAirport(request);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Update_Found_Returns204()
        {
            var request = new UpdateAirportRequest(1, "DEL", "Indira Gandhi", "Delhi", "IN", true);
            _service.Setup(s => s.UpdateAirportAsync(request)).ReturnsAsync(true);

            var result = await _controller.UpdateAirport(request);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_NotFound_Returns404()
        {
            _service.Setup(s => s.DeleteAirportAsync(99)).ReturnsAsync(false);

            var result = await _controller.DeleteAirportById(99);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Delete_Found_Returns204()
        {
            _service.Setup(s => s.DeleteAirportAsync(1)).ReturnsAsync(true);

            var result = await _controller.DeleteAirportById(1);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }
    }
}
