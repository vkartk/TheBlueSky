using AutoMapper;
using TheBlueSky.Bookings.DTOs.Requests.Passenger;
using TheBlueSky.Bookings.DTOs.Responses.Passenger;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories.Interfaces;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IPassengerRepository _repository;
        private readonly IMapper _mapper;

        public PassengerService(IPassengerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PassengerResponse>> GetAllAsync()
        {
            var passengers = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<PassengerResponse>>(passengers);
        }

        public async Task<PassengerResponse?> GetByIdAsync(int passengerId)
        {
            var passenger = await _repository.GetByIdAsync(passengerId);
            return passenger is null ? null : _mapper.Map<PassengerResponse>(passenger);
        }

        public async Task<PassengerResponse> CreateAsync(CreatePassengerRequest request)
        {
            var passenger = _mapper.Map<Passenger>(request);
            var created = await _repository.AddAsync(passenger);
            return _mapper.Map<PassengerResponse>(created);
        }

        public async Task<bool> UpdateAsync(UpdatePassengerRequest request)
        {
            var passenger = _mapper.Map<Passenger>(request);
            return await _repository.UpdateAsync(passenger);
        }

        public Task<bool> DeleteAsync(int passengerId)
        {
            return _repository.DeleteAsync(passengerId);
        }
    }
}
