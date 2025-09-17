using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TheBlueSky.Flights.DTOs.Requests.Airport;
using TheBlueSky.Flights.Mappings;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories.Interfaces;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Tests.Services
{
    [TestFixture]
    public class AirportServiceTests
    {
        private Mock<IAirportRepository> _repo = null!;
        private IMapper _mapper = null!;
        private AirportService _service = null!;

        [SetUp]
        public void SetUp()
        {
            var loggerFactory = LoggerFactory.Create(b => b.SetMinimumLevel(LogLevel.Warning));
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AirportProfile());
            }, loggerFactory);
            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();

            _repo = new Mock<IAirportRepository>();
            _service = new AirportService(_repo.Object, _mapper);
        }

        [Test]
        public async Task GetAll_ReturnsList()
        {
            var airports = new List<Airport>
            {
                new() { AirportId = 1, AirportCode = "DEL", AirportName = "Indira Gandhi", City = "Delhi", CountryId = "IN" }
            };
            _repo.Setup(r => r.GetAllAirportsAsync()).ReturnsAsync(airports);

            var result = await _service.GetAllAirportsAsync();

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().AirportCode, Is.EqualTo("DEL"));
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNull()
        {
            _repo.Setup(r => r.GetAirportByIdAsync(99)).ReturnsAsync((Airport?)null);

            var result = await _service.GetAirportByIdAsync(99);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Create_ReturnsCreatedAirport()
        {
            var request = new CreateAirportRequest("DEL", "Indira Gandhi", "Delhi", "IN");
            var created = new Airport { AirportId = 1, AirportCode = "DEL", AirportName = "Indira Gandhi", City = "Delhi", CountryId = "IN" };

            _repo.Setup(r => r.AddAirportAsync(It.IsAny<Airport>())).ReturnsAsync(created);

            var result = await _service.CreateAirportAsync(request);

            Assert.That(result.AirportId, Is.EqualTo(1));
        }

        [Test]
        public async Task Update_NotFound_ReturnsFalse()
        {
            var request = new UpdateAirportRequest(99, "DEL", "Indira Gandhi", "Delhi", "IN", true);
            _repo.Setup(r => r.GetAirportByIdAsync(99)).ReturnsAsync((Airport?)null);

            var ok = await _service.UpdateAirportAsync(request);

            Assert.That(ok, Is.False);
        }

        [Test]
        public async Task Update_Found_ReturnsTrue()
        {
            var existing = new Airport { AirportId = 1, AirportCode = "DEL", AirportName = "Indira Gandhi", City = "Delhi", CountryId = "IN" };
            var request = new UpdateAirportRequest(1, "DEL", "Indira Gandhi", "Delhi", "IN", true);

            _repo.Setup(r => r.GetAirportByIdAsync(1)).ReturnsAsync(existing);
            _repo.Setup(r => r.UpdateAirportAsync(It.IsAny<Airport>())).ReturnsAsync(true);

            var ok = await _service.UpdateAirportAsync(request);

            Assert.That(ok, Is.True);
        }
    }
}
