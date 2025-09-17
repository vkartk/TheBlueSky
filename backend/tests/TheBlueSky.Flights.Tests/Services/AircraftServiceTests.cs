using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TheBlueSky.Flights.DTOs.Requests.Aircraft;
using TheBlueSky.Flights.DTOs.Responses.Aircraft;
using TheBlueSky.Flights.Enums;
using TheBlueSky.Flights.Mappings;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories.Interfaces;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Tests.Services
{
    [TestFixture]
    public class AircraftServiceTests
    {
        private Mock<IAircraftRepository> _repoMock = null!;
        private IMapper _mapper = null!;
        private ILoggerFactory _loggerFactory = null!;
        private AircraftService _sut = null!;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _repoMock = new Mock<IAircraftRepository>(MockBehavior.Strict);

            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .SetMinimumLevel(LogLevel.Debug)
                    .AddDebug()
                    .AddConsole();
            });

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AircraftProfile());
            }, _loggerFactory);

            _mapper = mappingConfig.CreateMapper();

            _sut = new AircraftService(_repoMock.Object, _mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _loggerFactory?.Dispose();
        }

        [Test]
        public void AutoMapper_Configuration_IsValid()
        {
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AircraftProfile());
            }, _loggerFactory);

            Assert.DoesNotThrow(() => mappingConfig.AssertConfigurationIsValid());
        }

        [Test]
        public async Task GetAllAircraftsAsync_HappyPath_ReturnsMappedDtos()
        {
            // Arrange
            var entities = new List<Aircraft>
            {
                new()
                {
                    AircraftId = 1, OwnerUserId = 10, AircraftName = "A320",
                    AircraftModel = "A320neo", Manufacturer = AircraftManufacturer.Airbus,
                    EconomySeats = 150, BusinessSeats = 12, FirstClassSeats = 0, IsActive = true,
                    CreatedDate = DateTime.UtcNow
                }
            };
            _repoMock.Setup(r => r.GetAllAircraftsAsync()).ReturnsAsync(entities);

            // Act
            var result = await _sut.GetAllAircraftsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            var list = new List<AircraftResponse>(result);
            Assert.That(list, Has.Count.EqualTo(1));
            Assert.That(list[0].AircraftName, Is.EqualTo("A320"));
            _repoMock.Verify(r => r.GetAllAircraftsAsync(), Times.Once);
        }

        [Test]
        public async Task GetAircraftByIdAsync_NotFound_ReturnsNull()
        {
            // Arrange
            _repoMock.Setup(r => r.GetAircraftByIdAsync(999)).ReturnsAsync((Aircraft?)null);

            // Act
            var result = await _sut.GetAircraftByIdAsync(999);

            // Assert
            Assert.That(result, Is.Null);
            _repoMock.Verify(r => r.GetAircraftByIdAsync(999), Times.Once);
        }

        [Test]
        public async Task CreateAircraftAsync_ValidRequest_ReturnsCreatedDto()
        {
            // Arrange
            var request = new CreateAircraftRequest(
                OwnerUserId: 10,
                AircraftName: "737",
                AircraftModel: "737-800",
                Manufacturer: AircraftManufacturer.Boeing,
                EconomySeats: 160,
                BusinessSeats: 8,
                FirstClassSeats: 0
            );

            var entity = new Aircraft
            {
                AircraftId = 42,
                OwnerUserId = 10,
                AircraftName = "737",
                AircraftModel = "737-800",
                Manufacturer = AircraftManufacturer.Boeing,
                EconomySeats = 160,
                BusinessSeats = 8,
                FirstClassSeats = 0,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            _repoMock
                .Setup(r => r.AddAircraftAsync(It.IsAny<Aircraft>()))
                .ReturnsAsync(entity);

            // Act
            var result = await _sut.CreateAircraftAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AircraftId, Is.EqualTo(42));
            Assert.That(result.Manufacturer, Is.EqualTo(AircraftManufacturer.Boeing));
            _repoMock.Verify(r => r.AddAircraftAsync(It.Is<Aircraft>(a => a.AircraftName == "737")), Times.Once);
        }

        [Test]
        public async Task UpdateAircraftAsync_NotFound_ReturnsFalse()
        {
            // Arrange
            var update = new UpdateAircraftRequest(
                AircraftId: 123,
                AircraftName: "A350",
                AircraftModel: "A350-900",
                Manufacturer: AircraftManufacturer.Airbus,
                EconomySeats: 250,
                BusinessSeats: 36,
                FirstClassSeats: 8,
                IsActive: true
            );

            _repoMock.Setup(r => r.GetAircraftByIdAsync(123)).ReturnsAsync((Aircraft?)null);

            // Act
            var updated = await _sut.UpdateAircraftAsync(update);

            // Assert
            Assert.That(updated, Is.False);
            _repoMock.Verify(r => r.GetAircraftByIdAsync(123), Times.Once);
            _repoMock.Verify(r => r.UpdateAircraftAsync(It.IsAny<Aircraft>()), Times.Never);
        }

        [Test]
        public async Task UpdateAircraftAsync_HappyPath_MapsAndCallsRepo()
        {
            // Arrange
            var existing = new Aircraft
            {
                AircraftId = 1,
                OwnerUserId = 77,
                AircraftName = "Old",
                AircraftModel = "OldModel",
                Manufacturer = AircraftManufacturer.Airbus,
                EconomySeats = 100,
                BusinessSeats = 10,
                FirstClassSeats = 2,
                IsActive = true
            };

            var update = new UpdateAircraftRequest(
                AircraftId: 1,
                AircraftName: "NewName",
                AircraftModel: "NewModel",
                Manufacturer: AircraftManufacturer.Boeing,
                EconomySeats: 120,
                BusinessSeats: 14,
                FirstClassSeats: 4,
                IsActive: false
            );

            _repoMock.Setup(r => r.GetAircraftByIdAsync(1)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.UpdateAircraftAsync(existing)).ReturnsAsync(true);

            // Act
            var ok = await _sut.UpdateAircraftAsync(update);

            // Assert
            Assert.That(ok, Is.True);
            Assert.That(existing.AircraftName, Is.EqualTo("NewName"));
            Assert.That(existing.Manufacturer, Is.EqualTo(AircraftManufacturer.Boeing));
            _repoMock.Verify(r => r.UpdateAircraftAsync(existing), Times.Once);
        }

        [Test]
        public void CreateAircraftAsync_RepoThrows_PropagatesException()
        {
            // Arrange
            var request = new CreateAircraftRequest(
                OwnerUserId: 1,
                AircraftName: "X",
                AircraftModel: "Y",
                Manufacturer: AircraftManufacturer.Airbus,
                EconomySeats: 1,
                BusinessSeats: 0,
                FirstClassSeats: 0
            );

            _repoMock
                .Setup(r => r.AddAircraftAsync(It.IsAny<Aircraft>()))
                .ThrowsAsync(new InvalidOperationException("boom"));

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _sut.CreateAircraftAsync(request));
            _repoMock.Verify(r => r.AddAircraftAsync(It.IsAny<Aircraft>()), Times.Once);
        }
    }
}
