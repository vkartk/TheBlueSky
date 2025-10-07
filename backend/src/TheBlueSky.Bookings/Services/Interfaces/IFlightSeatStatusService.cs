using TheBlueSky.Bookings.DTOs.Requests.Flight;

namespace TheBlueSky.Bookings.Services.Interfaces
{
    public interface IFlightSeatStatusService
    {
        Task UpdateSeatStatusAsync(IEnumerable<int> seatStatusIds, string newStatus);

    }
}
