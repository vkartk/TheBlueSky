using TheBlueSky.Bookings.DTOs.Requests.Payment;
using TheBlueSky.Bookings.DTOs.Responses.Payment;

namespace TheBlueSky.Bookings.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentResponse>> GetAllAsync();
        Task<PaymentResponse?> GetByIdAsync(int paymentId);
        Task<PaymentResponse> CreateAsync(CreatePaymentRequest request);
        Task<bool> UpdateAsync(UpdatePaymentRequest request);
        Task<bool> DeleteAsync(int paymentId);

    }
}
