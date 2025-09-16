using Microsoft.EntityFrameworkCore;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories.Interfaces;

namespace TheBlueSky.Bookings.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly BookingsDbContext _context;

        public PaymentRepository(BookingsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments
                .AsNoTracking()
                .OrderByDescending(p => p.PaymentId)
                .ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(int paymentId)
        {
            return await _context.Payments
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }

        public async Task<Payment> AddAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<bool> UpdateAsync(Payment payment)
        {
            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(payment.PaymentId))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null) return false;

            _context.Payments.Remove(payment);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(paymentId))
                {
                    return false;
                }
                throw;
            }
        }

        public Task<bool> ExistsAsync(int paymentId)
        {
            return _context.Payments.AnyAsync(p => p.PaymentId == paymentId);
        }
    }
}
