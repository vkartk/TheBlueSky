using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TheBlueSky.Flights.DTOs.Requests.FlightSchedule;
using TheBlueSky.Flights.Mappings;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories.Interfaces;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Tests.Services
{
    [TestFixture]
    public class FlightScheduleServiceTests
    {
        private Mock<IFlightScheduleRepository> _repo = null!;
        private IMapper _mapper = null!;
        private FlightScheduleService _service = null!;

        [SetUp]
        public void SetUp()
        {
            var loggerFactory = LoggerFactory.Create(b => b.SetMinimumLevel(LogLevel.Warning));
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FlightScheduleProfile());
            }, loggerFactory);
            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();

            _repo = new Mock<IFlightScheduleRepository>();
            _service = new FlightScheduleService(_repo.Object, _mapper);
        }

        [Test]
        public async Task GetAll_ReturnsList()
        {
            // Arrange
            var list = new List<FlightSchedule>
            {
                new()
                {
                    FlightScheduleId = 1, AircraftId = 10, RouteId = 20,
                    FlightNumber = "TB101",
                    DepartureTime = new TimeOnly(9, 0),
                    ArrivalTime = new TimeOnly(11, 30),
                    BaseFare = 4999.99m,
                    CheckinBaggageWeightKg = 15,
                    CabinBaggageWeightKg = 7,
                    ValidFrom = DateOnly.FromDateTime(DateTime.Today),
                    ValidUntil = DateOnly.FromDateTime(DateTime.Today.AddDays(30))
                }
            };
            _repo.Setup(r => r.GetAllFlightSchedulesAsync()).ReturnsAsync(list);

            // Act
            var result = await _service.GetAllFlightSchedulesAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().FlightNumber, Is.EqualTo("TB101"));
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNull()
        {
            // Arrange
            _repo.Setup(r => r.GetFlightScheduleByIdAsync(999)).ReturnsAsync((FlightSchedule?)null);

            // Act
            var dto = await _service.GetFlightScheduleByIdAsync(999);

            // Assert
            Assert.That(dto, Is.Null);
        }

        [Test]
        public async Task Create_ReturnsCreatedSchedule()
        {
            // Arrange
            var req = new CreateFlightScheduleRequest(
                AircraftId: 10,
                RouteId: 20,
                FlightNumber: "TB202",
                FlightName: "Test Flight",
                DepartureTime: new TimeOnly(10, 0),
                ArrivalTime: new TimeOnly(12, 0),
                BaseFare: 3500m,
                CheckinBaggageWeightKg: 15,
                CabinBaggageWeightKg: 7,
                ValidFrom: DateOnly.FromDateTime(DateTime.Today),
                ValidUntil: DateOnly.FromDateTime(DateTime.Today.AddDays(10))
            );

            var created = new FlightSchedule
            {
                FlightScheduleId = 5,
                AircraftId = 10,
                RouteId = 20,
                FlightNumber = "TB202",
                FlightName = "Test Flight",
                DepartureTime = req.DepartureTime,
                ArrivalTime = req.ArrivalTime,
                BaseFare = req.BaseFare,
                CheckinBaggageWeightKg = req.CheckinBaggageWeightKg,
                CabinBaggageWeightKg = req.CabinBaggageWeightKg,
                ValidFrom = req.ValidFrom,
                ValidUntil = req.ValidUntil
            };

            _repo.Setup(r => r.AddFlightScheduleAsync(It.IsAny<FlightSchedule>()))
                 .ReturnsAsync(created);

            // Act
            var dto = await _service.CreateFlightScheduleAsync(req);

            // Assert
            Assert.That(dto.FlightScheduleId, Is.EqualTo(5));
            Assert.That(dto.FlightNumber, Is.EqualTo("TB202"));
        }

        [Test]
        public async Task Update_NotFound_ReturnsFalse()
        {
            // Arrange
            var req = new UpdateFlightScheduleRequest(
                FlightScheduleId: 999,
                FlightNumber: "TB303",
                FlightName: "Updated",
                DepartureTime: new TimeOnly(8, 0),
                ArrivalTime: new TimeOnly(10, 0),
                BaseFare: 4000m,
                CheckinBaggageWeightKg: 20,
                CabinBaggageWeightKg: 7,
                ValidFrom: DateOnly.FromDateTime(DateTime.Today),
                ValidUntil: DateOnly.FromDateTime(DateTime.Today.AddDays(20)),
                IsActive: true
            );
            _repo.Setup(r => r.GetFlightScheduleByIdAsync(999)).ReturnsAsync((FlightSchedule?)null);

            // Act
            var ok = await _service.UpdateFlightScheduleAsync(req);

            // Assert
            Assert.That(ok, Is.False);
        }

        [Test]
        public async Task Update_Found_ReturnsTrue()
        {
            // Arrange
            var existing = new FlightSchedule
            {
                FlightScheduleId = 7,
                AircraftId = 10,
                RouteId = 20,
                FlightNumber = "TB404",
                DepartureTime = new TimeOnly(6, 0),
                ArrivalTime = new TimeOnly(8, 0),
                BaseFare = 3000m,
                CheckinBaggageWeightKg = 15,
                CabinBaggageWeightKg = 7,
                ValidFrom = DateOnly.FromDateTime(DateTime.Today),
                ValidUntil = DateOnly.FromDateTime(DateTime.Today.AddDays(5))
            };
            var req = new UpdateFlightScheduleRequest(
                FlightScheduleId: 7,
                FlightNumber: "TB404",
                FlightName: "Morning Run",
                DepartureTime: new TimeOnly(6, 30),
                ArrivalTime: new TimeOnly(8, 30),
                BaseFare: 3200m,
                CheckinBaggageWeightKg: 15,
                CabinBaggageWeightKg: 7,
                ValidFrom: existing.ValidFrom,
                ValidUntil: existing.ValidUntil,
                IsActive: true
            );

            _repo.Setup(r => r.GetFlightScheduleByIdAsync(7)).ReturnsAsync(existing);
            _repo.Setup(r => r.UpdateFlightScheduleAsync(It.IsAny<FlightSchedule>())).ReturnsAsync(true);

            // Act
            var ok = await _service.UpdateFlightScheduleAsync(req);

            // Assert
            Assert.That(ok, Is.True);
        }
    }
}
