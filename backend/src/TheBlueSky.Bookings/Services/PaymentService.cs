using AutoMapper;
using TheBlueSky.Bookings.DTOs.Requests.Payment;
using TheBlueSky.Bookings.DTOs.Responses.Payment;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories.Interfaces;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentResponse>> GetAllAsync()
        {
            var payments = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<PaymentResponse>>(payments);
        }

        public async Task<PaymentResponse?> GetByIdAsync(int paymentId)
        {
            var payment = await _repository.GetByIdAsync(paymentId);

            if (payment == null) return null;

            return _mapper.Map<PaymentResponse>(payment);
        }

        public async Task<PaymentResponse> CreateAsync(CreatePaymentRequest request)
        {
            var payment = _mapper.Map<Payment>(request);

            var created = await _repository.AddAsync(payment);

            return _mapper.Map<PaymentResponse>(created);
        }

        public async Task<bool> UpdateAsync(UpdatePaymentRequest request)
        {
            var payment = _mapper.Map<Payment>(request);

            return await _repository.UpdateAsync(payment);
        }

        public Task<bool> DeleteAsync(int paymentId)
        {
            return _repository.DeleteAsync(paymentId);
        }

    }
}
