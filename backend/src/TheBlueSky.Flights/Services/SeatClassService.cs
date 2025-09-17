using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.SeatClass;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories.Interfaces;

namespace TheBlueSky.Flights.Services
{
    public class SeatClassService : ISeatClassService
    {
        private readonly ISeatClassRepository _seatClassRepository;
        private readonly IMapper _mapper;

        public SeatClassService(ISeatClassRepository seatClassRepository, IMapper mapper)
        {
            _seatClassRepository = seatClassRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SeatClassDto>> GetAllSeatClassesAsync()
        {
            var seatClasses = await _seatClassRepository.GetAllSeatClassesAsync();
            return _mapper.Map<IEnumerable<SeatClassDto>>(seatClasses);
        }

        public async Task<SeatClassDto?> GetSeatClassByIdAsync(int id)
        {
            var seatClass = await _seatClassRepository.GetSeatClassByIdAsync(id);
            return _mapper.Map<SeatClassDto>(seatClass);
        }

        public async Task<SeatClassDto> CreateSeatClassAsync(CreateSeatClassRequest request)
        {
            var seatClass = _mapper.Map<SeatClass>(request);
            await _seatClassRepository.AddSeatClassAsync(seatClass);
            return _mapper.Map<SeatClassDto>(seatClass);
        }

        public async Task<bool> UpdateSeatClassAsync(int id, UpdateSeatClassRequest request)
        {
            var seatClass = await _seatClassRepository.GetSeatClassByIdAsync(id);
            if (seatClass == null)
            {
                return false;
            }

            _mapper.Map(request, seatClass);

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
