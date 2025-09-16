using AutoMapper;
using TheBlueSky.Bookings.DTOs.Requests.BookingPassenger;
using TheBlueSky.Bookings.DTOs.Responses.BookingPassenger;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories.Interfaces;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Services
{
    public class BookingPassengerService : IBookingPassengerService
    {
        private readonly IBookingPassengerRepository _repository;
        private readonly IMapper _mapper;

        public BookingPassengerService(IBookingPassengerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookingPassengerResponse>> GetAllAsync()
        {
            var bookingPassengers = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<BookingPassengerResponse>>(bookingPassengers);
        }

        public async Task<BookingPassengerResponse?> GetByIdAsync(int bookingPassengerId)
        {
            var bookingPassenger = await _repository.GetByIdAsync(bookingPassengerId);
            return bookingPassenger is null ? null : _mapper.Map<BookingPassengerResponse>(bookingPassenger);
        }

        public async Task<BookingPassengerResponse> CreateAsync(CreateBookingPassengerRequest request)
        {
            var bookingPassenger = _mapper.Map<BookingPassenger>(request);
            var createdBookingPassenger = await _repository.AddAsync(bookingPassenger);
            return _mapper.Map<BookingPassengerResponse>(createdBookingPassenger);
        }

        public async Task<bool> UpdateAsync(UpdateBookingPassengerRequest request)
        {
            var bookingPassenger = _mapper.Map<BookingPassenger>(request);
            return await _repository.UpdateAsync(bookingPassenger);
        }

        public Task<bool> DeleteAsync(int bookingPassengerId)
        {
            return _repository.DeleteAsync(bookingPassengerId);
        }


    }
}
