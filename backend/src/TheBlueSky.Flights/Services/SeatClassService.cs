using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories;

namespace TheBlueSky.Flights.Services
{
    public class SeatClassService : ISeatClassService
    {
        private readonly ISeatClassRepository _seatClassRepository;

        public SeatClassService(ISeatClassRepository seatClassRepository)
        {
            _seatClassRepository = seatClassRepository;
        }

        public async Task<IEnumerable<SeatClass>> GetAllSeatClassesAsync()
        {
            return await _seatClassRepository.GetAllSeatClassesAsync();
        }

        public async Task<SeatClass?> GetSeatClassByIdAsync(int id)
        {
            return await _seatClassRepository.GetSeatClassByIdAsync(id);
        }

        public async Task<SeatClass> CreateSeatClassAsync(SeatClass seatClass)
        {
            await _seatClassRepository.AddSeatClassAsync(seatClass);
            return seatClass;
        }

        public async Task<bool> UpdateSeatClassAsync(int id, SeatClass seatClass)
        {
            if (id != seatClass.SeatClassId)
            {
                return false;
            }

            var exists = await _seatClassRepository.SeatClassExists(id);
            if (!exists)
            {
                return false;
            }

            await _seatClassRepository.UpdateSeatClassAsync(seatClass);
            return true;
        }

        public async Task<bool> DeleteSeatClassAsync(int id)
        {
            var exists = await _seatClassRepository.SeatClassExists(id);
            if (!exists)
            {
                return false;
            }

            await _seatClassRepository.DeleteSeatClassAsync(id);
            return true;
        }
    }
}
