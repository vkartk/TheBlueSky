using AutoMapper;
using Moq;
using TheBlueSky.Bookings.DTOs.Requests.Passenger;
using TheBlueSky.Bookings.DTOs.Responses.Passenger;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories.Interfaces;
using TheBlueSky.Bookings.Services;

namespace TheBlueSky.Bookings.Tests.Services
{
    [TestFixture]
    public class PassengerServiceTests
    {
        private Mock<IPassengerRepository> _repo = null!;
        private Mock<IMapper> _mapper = null!;
        private PassengerService _sut = null!;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _repo = new Mock<IPassengerRepository>(MockBehavior.Strict);
            _mapper = new Mock<IMapper>(MockBehavior.Strict);
            _sut = new PassengerService(_repo.Object, _mapper.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _repo.VerifyAll();
            _mapper.VerifyAll();
        }

        [Test]
        public async Task GetAll_ReturnsList()
        {
            // Arrange
            var entities = new List<Passenger>
            {
                new() { PassengerId = 1, ManagedByUserId = 10, FirstName = "A", LastName = "L", DateOfBirth = new DateTime(2000,1,1), CreatedDate = new DateTime(2023,1,1) },
                new() { PassengerId = 2, ManagedByUserId = 11, FirstName = "B", LastName = "M", DateOfBirth = new DateTime(1999,1,1), CreatedDate = new DateTime(2024,1,1) }
            };

            var dto1 = new PassengerResponse(
                PassengerId: 1,
                ManagedByUserId: 10,
                FirstName: "A",
                LastName: "L",
                DateOfBirth: new DateTime(2000, 1, 1),
                Gender: null,
                PassportNumber: null,
                NationalityCountryId: null,
                RelationshipToManager: null,
                CreatedDate: new DateTime(2023, 1, 1),
                IsActive: true
            );

            var dto2 = new PassengerResponse(
                PassengerId: 2,
                ManagedByUserId: 11,
                FirstName: "B",
                LastName: "M",
                DateOfBirth: new DateTime(1999, 1, 1),
                Gender: null,
                PassportNumber: null,
                NationalityCountryId: null,
                RelationshipToManager: null,
                CreatedDate: new DateTime(2024, 1, 1),
                IsActive: true
            );

            var dtos = new List<PassengerResponse> { dto1, dto2 };

            _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
            _mapper.Setup(m => m.Map<IEnumerable<PassengerResponse>>(entities)).Returns(dtos);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Exactly(2).Items);
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNull()
        {
            // Arrange
            _repo.Setup(r => r.GetByIdAsync(123)).ReturnsAsync((Passenger?)null);

            // Act
            var res = await _sut.GetByIdAsync(123);

            // Assert
            Assert.That(res, Is.Null);
        }

        [Test]
        public async Task Create_ReturnsCreatedItem()
        {
            // Arrange
            var req = new CreatePassengerRequest(
                ManagedByUserId: 7,
                FirstName: "John",
                LastName: "Doe",
                DateOfBirth: new DateTime(1990, 5, 1),
                Gender: "M",
                PassportNumber: "P123",
                NationalityCountryId: "IN",
                RelationshipToManager: "Self"
            );

            var toAdd = new Passenger
            {
                ManagedByUserId = 7,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 5, 1),
                Gender = "M",
                PassportNumber = "P123",
                NationalityCountryId = "IN",
                RelationshipToManager = "Self",
                CreatedDate = new DateTime(2025, 1, 1),
                IsActive = true
            };

            var added = new Passenger
            {
                PassengerId = 42,
                ManagedByUserId = 7,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 5, 1),
                Gender = "M",
                PassportNumber = "P123",
                NationalityCountryId = "IN",
                RelationshipToManager = "Self",
                CreatedDate = new DateTime(2025, 1, 1),
                IsActive = true
            };

            var dto = new PassengerResponse(
                PassengerId: 42,
                ManagedByUserId: 7,
                FirstName: "John",
                LastName: "Doe",
                DateOfBirth: new DateTime(1990, 5, 1),
                Gender: "M",
                PassportNumber: "P123",
                NationalityCountryId: "IN",
                RelationshipToManager: "Self",
                CreatedDate: new DateTime(2025, 1, 1),
                IsActive: true
            );

            _mapper.Setup(m => m.Map<Passenger>(req)).Returns(toAdd);
            _repo.Setup(r => r.AddAsync(toAdd)).ReturnsAsync(added);
            _mapper.Setup(m => m.Map<PassengerResponse>(added)).Returns(dto);

            // Act
            var created = await _sut.CreateAsync(req);

            // Assert
            Assert.That(created, Is.Not.Null);
            Assert.That(created.PassengerId, Is.EqualTo(42));
            Assert.That(created.FirstName, Is.EqualTo("John"));
        }

        [Test]
        public async Task Update_Found_ReturnsTrue()
        {
            // Arrange
            var req = new UpdatePassengerRequest(
                PassengerId: 5,
                ManagedByUserId: 7,
                FirstName: "J",
                LastName: "D",
                DateOfBirth: new DateTime(1991, 1, 1),
                Gender: null,
                PassportNumber: null,
                NationalityCountryId: null,
                RelationshipToManager: null,
                IsActive: true
            );
            var entity = new Passenger { PassengerId = 5 };

            _mapper.Setup(m => m.Map<Passenger>(req)).Returns(entity);
            _repo.Setup(r => r.UpdateAsync(entity)).ReturnsAsync(true);

            // Act
            var ok = await _sut.UpdateAsync(req);

            // Assert
            Assert.That(ok, Is.True);
        }

        [Test]
        public async Task Update_NotFound_ReturnsFalse()
        {
            // Arrange
            var req = new UpdatePassengerRequest(
                PassengerId: 999,
                ManagedByUserId: 7,
                FirstName: "X",
                LastName: "Y",
                DateOfBirth: new DateTime(1991, 1, 1),
                Gender: null,
                PassportNumber: null,
                NationalityCountryId: null,
                RelationshipToManager: null,
                IsActive: true
            );
            var entity = new Passenger { PassengerId = 999 };

            _mapper.Setup(m => m.Map<Passenger>(req)).Returns(entity);
            _repo.Setup(r => r.UpdateAsync(entity)).ReturnsAsync(false);

            // Act
            var ok = await _sut.UpdateAsync(req);

            // Assert
            Assert.That(ok, Is.False);
        }

        [Test]
        public async Task Delete_Found_ReturnsTrue()
        {
            // Arrange
            _repo.Setup(r => r.DeleteAsync(10)).ReturnsAsync(true);

            // Act
            var ok = await _sut.DeleteAsync(10);

            // Assert
            Assert.That(ok, Is.True);
        }

        [Test]
        public void Create_WhenRepositoryFails_ThrowsException()
        {
            // Arrange
            var req = new CreatePassengerRequest(
                ManagedByUserId: 7,
                FirstName: "John",
                LastName: "Doe",
                DateOfBirth: new DateTime(1990, 5, 1),
                Gender: null,
                PassportNumber: null,
                NationalityCountryId: null,
                RelationshipToManager: null
            );

            var mapped = new Passenger();
            _mapper.Setup(m => m.Map<Passenger>(req)).Returns(mapped);
            _repo.Setup(r => r.AddAsync(mapped)).ThrowsAsync(new InvalidOperationException("Error"));

            // Act / Assert
            Assert.That(async () => await _sut.CreateAsync(req), Throws.InvalidOperationException);
        }
    }
}
