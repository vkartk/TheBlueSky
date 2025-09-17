using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TheBlueSky.Flights.DTOs.Requests.AircraftSeat;
using TheBlueSky.Flights.Mappings;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories.Interfaces;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Tests.Services
{
    [TestFixture]
    public class AircraftSeatServiceTests
    {
        private Mock<IAircraftSeatRepository> _repo = null!;
        private IMapper _mapper = null!;
        private AircraftSeatService _service = null!;

        [SetUp]
        public void SetUp()
        {
            var loggerFactory = LoggerFactory.Create(b => b.SetMinimumLevel(LogLevel.Warning));
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AircraftSeatProfile());
            }, loggerFactory);
            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();

            _repo = new Mock<IAircraftSeatRepository>();
            _service = new AircraftSeatService(_repo.Object, _mapper);
        }

        [Test]
        public async Task GetAll_ReturnsList()
        {
            var seats = new List<AircraftSeat>
            {
                new() { AircraftSeatId = 1, AircraftId = 10, SeatClassId = 2, SeatNumber = "1A", SeatPosition = "Window", AdditionalFare = 100, SeatRow = 1, SeatColumn = 1, IsActive = true },
                new() { AircraftSeatId = 2, AircraftId = 10, SeatClassId = 2, SeatNumber = "1B", SeatPosition = "Middle", AdditionalFare = 50, SeatRow = 1, SeatColumn = 2, IsActive = true }
            };
            _repo.Setup(r => r.GetAllAircraftSeatsAsync()).ReturnsAsync(seats);

            var result = await _service.GetAllAircraftSeatsAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNull()
        {
            _repo.Setup(r => r.GetAircraftSeatByIdAsync(999)).ReturnsAsync((AircraftSeat?)null);

            var result = await _service.GetAircraftSeatByIdAsync(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Create_ReturnsCreatedSeat()
        {
            var request = new CreateAircraftSeatRequest(10, 2, "2A", "Window", 75, 2, 1);
            var created = new AircraftSeat { AircraftSeatId = 123, AircraftId = 10, SeatClassId = 2, SeatNumber = "2A", SeatPosition = "Window", AdditionalFare = 75, SeatRow = 2, SeatColumn = 1, IsActive = true };

            _repo.Setup(r => r.AddAircraftSeatAsync(It.IsAny<AircraftSeat>())).ReturnsAsync(created);

            var result = await _service.CreateAircraftSeatAsync(request);

            Assert.That(result.AircraftSeatId, Is.EqualTo(123));
        }

        [Test]
        public async Task Update_NotFound_ReturnsFalse()
        {
            var request = new UpdateAircraftSeatRequest(999, "X", "Aisle", 0, 1, 1, true);
            _repo.Setup(r => r.GetAircraftSeatByIdAsync(999)).ReturnsAsync((AircraftSeat?)null);

            var ok = await _service.UpdateAircraftSeatAsync(request);

            Assert.That(ok, Is.False);
        }

        [Test]
        public async Task Update_ValidRequest_ReturnsTrue()
        {
            var existing = new AircraftSeat { AircraftSeatId = 5, AircraftId = 10, SeatClassId = 1, SeatNumber = "5C", SeatPosition = "Aisle", AdditionalFare = 10, SeatRow = 5, SeatColumn = 3, IsActive = true };
            var request = new UpdateAircraftSeatRequest(5, "5D", "Window", 20, 5, 4, false);

            _repo.Setup(r => r.GetAircraftSeatByIdAsync(5)).ReturnsAsync(existing);
            _repo.Setup(r => r.UpdateAircraftSeatAsync(It.IsAny<AircraftSeat>())).ReturnsAsync(true);

            var ok = await _service.UpdateAircraftSeatAsync(request);

            Assert.That(ok, Is.True);
        }

        [Test]
        public void Create_WhenRepositoryFails_ThrowsException()
        {
            // Arrange
            var request = new CreateAircraftSeatRequest(10, 2, "X", "Y", 1, 1, 1);
            _repo.Setup(r => r.AddAircraftSeatAsync(It.IsAny<AircraftSeat>()))
                 .ThrowsAsync(new System.InvalidOperationException("Error"));

            // Act + Assert
            Assert.That(async () => await _service.CreateAircraftSeatAsync(request),
                Throws.TypeOf<System.InvalidOperationException>().With.Message.EqualTo("Error"));
        }
    }
}
