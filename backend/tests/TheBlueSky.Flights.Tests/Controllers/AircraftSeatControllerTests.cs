using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TheBlueSky.Flights.Controllers;
using TheBlueSky.Flights.DTOs.Requests.AircraftSeat;
using TheBlueSky.Flights.DTOs.Responses.AircraftSeat;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Tests.Controllers
{
    [TestFixture]
    public class AircraftSeatControllerTests
    {
        private Mock<IAircraftSeatService> _service = null!;
        private AircraftSeatController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            _service = new Mock<IAircraftSeatService>();
            _controller = new AircraftSeatController(_service.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOk()
        {
            var list = new List<AircraftSeatResponse> { new(1, 10, 2, "1A", "Window", 100, 1, 1, true) };
            _service.Setup(s => s.GetAllAircraftSeatsAsync()).ReturnsAsync(list);

            var result = await _controller.GetAllAircraftSeats();

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetById_NotFound_Returns404()
        {
            _service.Setup(s => s.GetAircraftSeatByIdAsync(999)).ReturnsAsync((AircraftSeatResponse?)null);

            var result = await _controller.GetAircraftSeatById(999);

            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task GetById_Found_ReturnsOk()
        {
            var dto = new AircraftSeatResponse(5, 10, 1, "5C", "Aisle", 25, 5, 3, true);
            _service.Setup(s => s.GetAircraftSeatByIdAsync(5)).ReturnsAsync(dto);

            var result = await _controller.GetAircraftSeatById(5);

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task Create_InvalidModel_Returns400()
        {
            _controller.ModelState.AddModelError("SeatNumber", "Required");

            var result = await _controller.CreateAircraftSeat(new CreateAircraftSeatRequest(10, 1, "", "Window", 0, 1, 1));

            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Create_ValidRequest_Returns201()
        {
            var request = new CreateAircraftSeatRequest(10, 1, "2A", "Window", 50, 2, 1);
            var created = new AircraftSeatResponse(123, 10, 1, "2A", "Window", 50, 2, 1, true);

            _service.Setup(s => s.CreateAircraftSeatAsync(request)).ReturnsAsync(created);

            var result = await _controller.CreateAircraftSeat(request);

            Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());
        }

        [Test]
        public async Task Update_InvalidModel_Returns400()
        {
            _controller.ModelState.AddModelError("SeatNumber", "Required");
            var request = new UpdateAircraftSeatRequest(5, "", "Window", 0, 1, 1, true);

            var result = await _controller.UpdateAircraftSeat(request);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Update_NotFound_Returns404()
        {
            var request = new UpdateAircraftSeatRequest(999, "X", "Aisle", 0, 1, 1, true);
            _service.Setup(s => s.UpdateAircraftSeatAsync(request)).ReturnsAsync(false);

            var result = await _controller.UpdateAircraftSeat(request);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Update_Found_Returns204()
        {
            var request = new UpdateAircraftSeatRequest(5, "5D", "Window", 10, 5, 4, false);
            _service.Setup(s => s.UpdateAircraftSeatAsync(request)).ReturnsAsync(true);

            var result = await _controller.UpdateAircraftSeat(request);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_NotFound_Returns404()
        {
            _service.Setup(s => s.DeleteAircraftSeatAsync(321)).ReturnsAsync(false);

            var result = await _controller.DeleteAircraftSeatById(321);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Delete_Found_Returns204()
        {
            _service.Setup(s => s.DeleteAircraftSeatAsync(321)).ReturnsAsync(true);

            var result = await _controller.DeleteAircraftSeatById(321);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }
    }
}
