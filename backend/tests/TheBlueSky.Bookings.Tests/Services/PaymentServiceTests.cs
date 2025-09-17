using AutoMapper;
using Moq;
using TheBlueSky.Bookings.DTOs.Requests.Payment;
using TheBlueSky.Bookings.DTOs.Responses.Payment;
using TheBlueSky.Bookings.Enums;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories.Interfaces;
using TheBlueSky.Bookings.Services;

namespace TheBlueSky.Bookings.Tests.Services
{
    [TestFixture]
    public class PaymentServiceTests
    {
        private Mock<IPaymentRepository> _repo = null!;
        private Mock<IMapper> _mapper = null!;
        private PaymentService _sut = null!;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _repo = new Mock<IPaymentRepository>(MockBehavior.Strict);
            _mapper = new Mock<IMapper>(MockBehavior.Strict);
            _sut = new PaymentService(_repo.Object, _mapper.Object);
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
            var entities = new List<Payment>
            {
                new() { PaymentId = 1, BookingId = 10, PaymentMethod = PaymentMethod.Card, PaymentAmount = 1000m, PaymentStatus = PaymentStatus.Paid },
                new() { PaymentId = 2, BookingId = 11, PaymentMethod = PaymentMethod.Upi,  PaymentAmount =  500m, PaymentStatus = PaymentStatus.Pending }
            };

            var dtos = new List<PaymentResponse>
            {
                new(1, 10, PaymentMethod.Card, 1000m, DateTime.UtcNow, PaymentStatus.Paid, null, null, null),
                new(2, 11, PaymentMethod.Upi,  500m, null,             PaymentStatus.Pending, null, null, null)
            };

            _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
            _mapper.Setup(m => m.Map<IEnumerable<PaymentResponse>>(entities)).Returns(dtos);

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
            _repo.Setup(r => r.GetByIdAsync(123)).ReturnsAsync((Payment?)null);

            // Act
            var res = await _sut.GetByIdAsync(123);

            // Assert
            Assert.That(res, Is.Null);
        }

        [Test]
        public async Task Create_ReturnsCreatedItem()
        {
            // Arrange
            var req = new CreatePaymentRequest(
                BookingId: 7,
                PaymentMethod: PaymentMethod.Card,
                PaymentAmount: 1200m,
                PaymentDate: new DateTime(2025, 1, 1),
                PaymentStatus: PaymentStatus.Paid,
                GatewayTransactionId: "TXN123",
                RefundDate: null,
                RefundAmount: null
            );

            var toAdd = new Payment
            {
                BookingId = 7,
                PaymentMethod = PaymentMethod.Card,
                PaymentAmount = 1200m,
                PaymentDate = new DateTime(2025, 1, 1),
                PaymentStatus = PaymentStatus.Paid,
                GatewayTransactionId = "TXN123"
            };

            var added = new Payment
            {
                PaymentId = 42,
                BookingId = 7,
                PaymentMethod = PaymentMethod.Card,
                PaymentAmount = 1200m,
                PaymentDate = new DateTime(2025, 1, 1),
                PaymentStatus = PaymentStatus.Paid,
                GatewayTransactionId = "TXN123"
            };

            var dto = new PaymentResponse(
                PaymentId: 42,
                BookingId: 7,
                PaymentMethod: PaymentMethod.Card,
                PaymentAmount: 1200m,
                PaymentDate: new DateTime(2025, 1, 1),
                PaymentStatus: PaymentStatus.Paid,
                GatewayTransactionId: "TXN123",
                RefundDate: null,
                RefundAmount: null
            );

            _mapper.Setup(m => m.Map<Payment>(req)).Returns(toAdd);
            _repo.Setup(r => r.AddAsync(toAdd)).ReturnsAsync(added);
            _mapper.Setup(m => m.Map<PaymentResponse>(added)).Returns(dto);

            // Act
            var created = await _sut.CreateAsync(req);

            // Assert
            Assert.That(created.PaymentId, Is.EqualTo(42));
            Assert.That(created.PaymentAmount, Is.EqualTo(1200m));
        }

        [Test]
        public async Task Update_Found_ReturnsTrue()
        {
            // Arrange
            var req = new UpdatePaymentRequest(
                PaymentId: 5,
                BookingId: 7,
                PaymentMethod: PaymentMethod.Upi,
                PaymentAmount: 999m,
                PaymentDate: null,
                PaymentStatus: PaymentStatus.Paid,
                GatewayTransactionId: "UPI-1",
                RefundDate: null,
                RefundAmount: null
            );
            var entity = new Payment { PaymentId = 5 };

            _mapper.Setup(m => m.Map<Payment>(req)).Returns(entity);
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
            var req = new UpdatePaymentRequest(
                PaymentId: 999,
                BookingId: 7,
                PaymentMethod: PaymentMethod.Card,
                PaymentAmount: 100m,
                PaymentDate: null,
                PaymentStatus: PaymentStatus.Pending,
                GatewayTransactionId: null,
                RefundDate: null,
                RefundAmount: null
            );
            var entity = new Payment { PaymentId = 999 };

            _mapper.Setup(m => m.Map<Payment>(req)).Returns(entity);
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
            var req = new CreatePaymentRequest(
                BookingId: 7,
                PaymentMethod: PaymentMethod.Card,
                PaymentAmount: 1200m,
                PaymentDate: null,
                PaymentStatus: PaymentStatus.Paid,
                GatewayTransactionId: "TXN123",
                RefundDate: null,
                RefundAmount: null
            );

            var mapped = new Payment();
            _mapper.Setup(m => m.Map<Payment>(req)).Returns(mapped);
            _repo.Setup(r => r.AddAsync(mapped)).ThrowsAsync(new InvalidOperationException("boom"));

            // Act / Assert
            Assert.That(async () => await _sut.CreateAsync(req), Throws.InvalidOperationException);
        }
    }
}