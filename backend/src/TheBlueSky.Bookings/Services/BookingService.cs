using AutoMapper;
using TheBlueSky.Bookings.DTOs.Requests.Booking;
using TheBlueSky.Bookings.DTOs.Responses.Booking;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories.Interfaces;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repository;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookingResponse>> GetAllAsync()
        {
            var bookings = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<BookingResponse>>(bookings);
        }

        public async Task<BookingResponse?> GetByIdAsync(int id)
        {
            var booking = await _repository.GetByIdAsync(id);

            if(booking == null) return null;

            return _mapper.Map<BookingResponse>(booking);
        }

        public async Task<BookingResponse> CreateAsync(CreateBookingRequest request)
        {
            var booking = _mapper.Map<Booking>(request);
            booking = await _repository.AddAsync(booking);

            return _mapper.Map<BookingResponse>(booking);
        }

        public async Task<bool> UpdateAsync(UpdateBookingRequest request)
        {
            var booking = await _repository.GetByIdAsync(request.BookingId);

            if (booking is null) return false;

            _mapper.Map(request, booking);
            return await _repository.UpdateAsync(booking);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }

    }
}
