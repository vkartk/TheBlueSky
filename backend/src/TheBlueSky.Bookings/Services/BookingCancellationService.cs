using AutoMapper;
using TheBlueSky.Bookings.DTOs.Requests.BookingCancellation;
using TheBlueSky.Bookings.DTOs.Responses.BookingCancellation;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories.Interfaces;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Services
{
    public class BookingCancellationService : IBookingCancellationService
    {
        private readonly IBookingCancellationRepository _repository;
        private readonly IMapper _mapper;

        public BookingCancellationService(IBookingCancellationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookingCancellationResponse>> GetAllAsync()
        {
            var bookingCancellations = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<BookingCancellationResponse>>(bookingCancellations);
        }

        public async Task<BookingCancellationResponse?> GetByIdAsync(int bookingCancellationId)
        {
            var bookingCancellation = await _repository.GetByIdAsync(bookingCancellationId);

            if (bookingCancellation == null) return null;

            return _mapper.Map<BookingCancellationResponse>(bookingCancellation);
        }

        public async Task<BookingCancellationResponse> CreateAsync(CreateBookingCancellationRequest request)
        {
            var bookingCancellation = _mapper.Map<BookingCancellation>(request);

            var created = await _repository.AddAsync(bookingCancellation);

            return _mapper.Map<BookingCancellationResponse>(created);
        }

        public async Task<bool> UpdateAsync(UpdateBookingCancellationRequest request)
        {
            var bookingCancellation = _mapper.Map<BookingCancellation>(request);
            return await _repository.UpdateAsync(bookingCancellation);
        }

        public Task<bool> DeleteAsync(int bookingCancellationId)
        {
            return _repository.DeleteAsync(bookingCancellationId);
        }

    }
}
