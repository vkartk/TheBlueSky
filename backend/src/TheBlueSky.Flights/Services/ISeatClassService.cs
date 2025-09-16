using TheBlueSky.Flights.DTOs.Requests.SeatClass;

namespace TheBlueSky.Flights.Services
{
    public interface ISeatClassService
    {
        Task<IEnumerable<SeatClassDto>> GetAllSeatClassesAsync();
        Task<SeatClassDto?> GetSeatClassByIdAsync(int id);
        Task<SeatClassDto> CreateSeatClassAsync(CreateSeatClassRequest request);
        Task<bool> UpdateSeatClassAsync(int id, UpdateSeatClassRequest request);
        Task<bool> DeleteSeatClassAsync(int id);

    }
}
