using TheBlueSky.Bookings.Models;

namespace TheBlueSky.Bookings.Repositories.Interfaces
{
    public interface IPassengerRepository
    {
        Task<IEnumerable<Passenger>> GetAllAsync();
        Task<Passenger?> GetByIdAsync(int passengerId);
        Task<IEnumerable<Passenger>> GetByUserIdAsync(string userId);
        Task<Passenger> AddAsync(Passenger passenger);
        Task<bool> UpdateAsync(Passenger passenger);
        Task<bool> DeleteAsync(int passengerId);
        Task<bool> ExistsAsync(int passengerId);

    }
}
