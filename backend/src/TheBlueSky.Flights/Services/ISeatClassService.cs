using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Services
{
    public interface ISeatClassService
    {
        Task<IEnumerable<SeatClass>> GetAllSeatClassesAsync();
        Task<SeatClass?> GetSeatClassByIdAsync(int id);
        Task<SeatClass> CreateSeatClassAsync(SeatClass seatClass);
        Task<bool> UpdateSeatClassAsync(int id, SeatClass seatClass);
        Task<bool> DeleteSeatClassAsync(int id);

    }
}
