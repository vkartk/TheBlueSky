using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TheBlueSky.Flights.Controllers;
using TheBlueSky.Flights.DTOs.Requests.AircraftSeat;
using TheBlueSky.Flights.DTOs.Responses.AircraftSeat;
using TheBlueSky.Flights.Enums;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Tests.Controllers
{
    [TestFixture]
    public class AircraftSeatControllerTests
    {
        private Mock<IAircraftSeatService> _service = null!;
        private Mock<ILogger<AircraftSeatController>> _loggerMock = null!;
        private AircraftSeatController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            _service = new Mock<IAircraftSeatService>();
            _loggerMock = new Mock<ILogger<AircraftSeatController>>();
            _controller = new AircraftSeatController(_service.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAllAircraftSeats_WhenCalled_ShouldReturnOk()
        {
            var list = new List<AircraftSeatResponse> { new(1, 10, SeatClass.Economy, "1A", "Window", 100, 1, 1, true) };
            _service.Setup(s => s.GetAllAircraftSeatsAsync()).ReturnsAsync(list);

            var result = await _controller.GetAllAircraftSeats();

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetById_WhenIdIsUnknown_ShouldReturnNotFound()
        {
            _service.Setup(s => s.GetAircraftSeatByIdAsync(999)).ReturnsAsync((AircraftSeatResponse?)null);

            var result = await _controller.GetAircraftSeatById(999);

            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task GetById_WhenIdIsValid_ShouldReturnOk()
        {
            var dto = new AircraftSeatResponse(5, 10, SeatClass.Economy, "5C", "Aisle", 25, 5, 3, true);
            _service.Setup(s => s.GetAircraftSeatByIdAsync(5)).ReturnsAsync(dto);

            var result = await _controller.GetAircraftSeatById(5);

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task Create_WhenModelIsInvalid_ShouldReturnBadRequest()
        {
            _controller.ModelState.AddModelError("SeatNumber", "Required");

            var result = await _controller.CreateAircraftSeat(new CreateAircraftSeatRequest(10, SeatClass.Economy, "", "Window", 0, 1, 1));

            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Create_WhenRequestIsValid_ShouldReturnCreated()
        {
            var request = new CreateAircraftSeatRequest(10, SeatClass.Economy, "2A", "Window", 50, 2, 1);
            var created = new AircraftSeatResponse(123, 10, SeatClass.Economy, "2A", "Window", 50, 2, 1, true);

            _service.Setup(s => s.CreateAircraftSeatAsync(request)).ReturnsAsync(created);

            var result = await _controller.CreateAircraftSeat(request);

            Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());
        }

        [Test]
        public async Task Update_WhenModelIsInvalid_ShouldReturnBadRequest()
        {
            _controller.ModelState.AddModelError("SeatNumber", "Required");
            var request = new UpdateAircraftSeatRequest(5, "", "Window",SeatClass.Economy, 0, 1, 1, true);

            var result = await _controller.UpdateAircraftSeat(request.AircraftSeatId,request);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Update_WhenIdIsUnknown_ShouldReturnNotFound()
        {
            var request = new UpdateAircraftSeatRequest(999, "X", "Aisle", SeatClass.Economy, 0, 1, 1, true);
            _service.Setup(s => s.UpdateAircraftSeatAsync(request)).ReturnsAsync(false);

            var result = await _controller.UpdateAircraftSeat(request.AircraftSeatId, request);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Update_WhenIdIsValid_ShouldReturnNoContent()
        {
            var request = new UpdateAircraftSeatRequest(5, "5D", "Window", SeatClass.Economy, 10, 5, 4, false);
            _service.Setup(s => s.UpdateAircraftSeatAsync(request)).ReturnsAsync(true);

            var result = await _controller.UpdateAircraftSeat(request.AircraftSeatId, request);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_WhenIdIsUnknown_ShouldReturnNotFound()
        {
            _service.Setup(s => s.DeleteAircraftSeatAsync(321)).ReturnsAsync(false);

            var result = await _controller.DeleteAircraftSeatById(321);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Delete_WhenIdIsValid_ShouldReturnNoContent()
        {
            _service.Setup(s => s.DeleteAircraftSeatAsync(321)).ReturnsAsync(true);

            var result = await _controller.DeleteAircraftSeatById(321);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }
    }
}
