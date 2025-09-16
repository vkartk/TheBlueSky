using TheBlueSky.Bookings.Models;

namespace TheBlueSky.Bookings.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(int paymentId);
        Task<Payment> AddAsync(Payment payment);
        Task<bool> UpdateAsync(Payment payment);
        Task<bool> DeleteAsync(int paymentId);
        Task<bool> ExistsAsync(int paymentId);

    }
}
