using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Repositories.Interfaces
{
    public interface ISeatClassRepository
    {
        Task<IEnumerable<SeatClass>> GetAllSeatClassesAsync();
        Task<SeatClass?> GetSeatClassByIdAsync(int id);
        Task AddSeatClassAsync(SeatClass seatClass);
        Task UpdateSeatClassAsync(SeatClass seatClass);
        Task DeleteSeatClassAsync(int id);
        Task<bool> SeatClassExists(int id);

    }
}
