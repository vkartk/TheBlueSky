using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TheBlueSky.Flights.DTOs.Requests.Flight;
using TheBlueSky.Flights.Enums;
using TheBlueSky.Flights.Mappings;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories.Interfaces;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Tests.Services
{
    [TestFixture]
    public class FlightServiceTests
    {
        private Mock<IFlightRepository> _repo = null!;
        private IMapper _mapper = null!;
        private FlightService _service = null!;

        [SetUp]
        public void SetUp()
        {
            var loggerFactory = LoggerFactory.Create(b => b.SetMinimumLevel(LogLevel.Warning));
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FlightProfile());
            }, loggerFactory);
            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();

            _repo = new Mock<IFlightRepository>();
            _service = new FlightService(_repo.Object, _mapper);
        }

        [Test]
        public async Task GetAll_ReturnsList()
        {
            var flights = new List<Flight>
            {
                new() { FlightId = 1, FlightScheduleId = 10, FlightDate = DateOnly.FromDateTime(DateTime.Today), DepartureDateTime = DateTimeOffset.UtcNow, ArrivalDateTime = DateTimeOffset.UtcNow.AddHours(2), FlightStatus = FlightStatus.Scheduled }
            };
            _repo.Setup(r => r.GetAllFlightsAsync()).ReturnsAsync(flights);

            var result = await _service.GetAllFlightsAsync();

            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNull()
        {
            _repo.Setup(r => r.GetFlightByIdAsync(99)).ReturnsAsync((Flight?)null);

            var result = await _service.GetFlightByIdAsync(99);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Create_ReturnsCreatedFlight()
        {
            var request = new CreateFlightRequest(10, DateOnly.FromDateTime(DateTime.Today), DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(2), FlightStatus.Scheduled, 100);
            var created = new Flight { FlightId = 1, FlightScheduleId = 10, FlightDate = request.FlightDate, DepartureDateTime = request.DepartureDateTime, ArrivalDateTime = request.ArrivalDateTime, FlightStatus = request.FlightStatus, AvailableSeats = 100 };

            _repo.Setup(r => r.AddFlightAsync(It.IsAny<Flight>())).ReturnsAsync(created);

            var result = await _service.CreateFlightAsync(request);

            Assert.That(result.FlightId, Is.EqualTo(1));
        }

        [Test]
        public async Task Update_NotFound_ReturnsFalse()
        {
            var request = new UpdateFlightRequest(99, DateOnly.FromDateTime(DateTime.Today), DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(2), FlightStatus.Scheduled, 100);
            _repo.Setup(r => r.GetFlightByIdAsync(99)).ReturnsAsync((Flight?)null);

            var ok = await _service.UpdateFlightAsync(request);

            Assert.That(ok, Is.False);
        }

        [Test]
        public async Task Update_Found_ReturnsTrue()
        {
            var existing = new Flight { FlightId = 1, FlightScheduleId = 10, FlightDate = DateOnly.FromDateTime(DateTime.Today), DepartureDateTime = DateTimeOffset.UtcNow, ArrivalDateTime = DateTimeOffset.UtcNow.AddHours(2), FlightStatus = FlightStatus.Scheduled };
            var request = new UpdateFlightRequest(1, existing.FlightDate, existing.DepartureDateTime, existing.ArrivalDateTime, FlightStatus.Boarding, 80);

            _repo.Setup(r => r.GetFlightByIdAsync(1)).ReturnsAsync(existing);
            _repo.Setup(r => r.UpdateFlightAsync(It.IsAny<Flight>())).ReturnsAsync(true);

            var ok = await _service.UpdateFlightAsync(request);

            Assert.That(ok, Is.True);
        }
    }
}
