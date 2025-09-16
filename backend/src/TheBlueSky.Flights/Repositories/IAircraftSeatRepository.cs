using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Repositories
{
    public interface IAircraftSeatRepository
    {
        Task<IEnumerable<AircraftSeat>> GetAllAircraftSeatsAsync();
        Task<AircraftSeat?> GetAircraftSeatByIdAsync(int id);
        Task<AircraftSeat> AddAircraftSeatAsync(AircraftSeat aircraftSeat);
        Task<bool> UpdateAircraftSeatAsync(AircraftSeat aircraftSeat);
        Task<bool> DeleteAircraftSeatAsync(int id);
        Task<bool> ExistsAsync(int id);

    }
}
