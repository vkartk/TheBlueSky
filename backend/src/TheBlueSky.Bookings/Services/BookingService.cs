using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TheBlueSky.Bookings.DTOs.Requests.Booking;
using TheBlueSky.Bookings.DTOs.Responses.Booking;
using TheBlueSky.Bookings.Enums;
using TheBlueSky.Bookings.Models;
using TheBlueSky.Bookings.Repositories;
using TheBlueSky.Bookings.Repositories.Interfaces;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repository;
        private readonly IBookingPassengerRepository _passengerRepository;
        private readonly IFlightSeatStatusService _flightSeatStatusService;
        private readonly BookingsDbContext _context;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository repository, IBookingPassengerRepository passengerRepository, IFlightSeatStatusService flightSeatStatusService, BookingsDbContext context, IMapper mapper)
        {
            _repository = repository;
            _flightSeatStatusService = flightSeatStatusService;
            _passengerRepository = passengerRepository;
            _context = context;
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

        public async Task<IEnumerable<BookingResponse>> GetByUserIdAsync(string userId)
        {
            var bookings = await _repository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<BookingResponse>>(bookings);
        }


        public async Task<BookingResponse> CreateAsync(CreateBookingRequest request)
        { 

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var booking = _mapper.Map<Booking>(request);

                booking.UserId = request.UserId;
                booking.BookingDate = DateTime.UtcNow;
                booking.NumberOfPassengers = request.PassengerSeatSelections.Count;
                booking.BookingStatus = BookingStatus.Confirmed;
                booking.PaymentStatus = PaymentStatus.Paid;

                var createdBooking = await _repository.AddAsync(booking);
                await _context.SaveChangesAsync();

                var bookingPassengers = request.PassengerSeatSelections.Select(p => new BookingPassenger
                {
                    BookingId = createdBooking.BookingId,
                    PassengerId = p.PassengerId,
                    TicketNumber = p.TicketNumber,
                    TicketPrice = p.TicketPrice,
                    FlightSeatStatusId = p.FlightSeatStatusId,
                }).ToList();

                await _passengerRepository.AddRangeAsync(bookingPassengers);
                await _context.SaveChangesAsync();

                var seatStatusIds = request.PassengerSeatSelections.Select(p => p.FlightSeatStatusId);
                await _flightSeatStatusService.UpdateSeatStatusAsync(seatStatusIds, "Reserved");

                await transaction.CommitAsync();



                return _mapper.Map<BookingResponse>(createdBooking);



            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

        public async Task<bool> UpdateAsync(int id,UpdateBookingRequest request)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                var booking = await _repository.GetByIdAsync(id);

                if (booking is null) return false;

                if (request.BookingStatus == BookingStatus.Cancelled && booking.BookingStatus != BookingStatus.Cancelled)
                {
                    var bookingPassengers = await _context.BookingPassengers
                                                          .Where(p => p.BookingId == id)
                                                          .ToListAsync();

                    if (bookingPassengers.Any())
                    {
                        var seatStatusIds = bookingPassengers.Select(p => p.FlightSeatStatusId);
                        await _flightSeatStatusService.UpdateSeatStatusAsync(seatStatusIds, "Available");
                    }
                }


                _mapper.Map(request, booking);

                var success = await _repository.UpdateAsync(booking);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return success;

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }

    }
}
