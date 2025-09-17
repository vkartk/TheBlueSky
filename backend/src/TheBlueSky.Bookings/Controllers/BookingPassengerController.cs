using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheBlueSky.Bookings.DTOs.Requests.BookingPassenger;
using TheBlueSky.Bookings.DTOs.Responses.BookingPassenger;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingPassengerController : ControllerBase
    {
        private readonly IBookingPassengerService _service;
        private readonly ILogger<BookingPassengerController> _logger;

        public BookingPassengerController(IBookingPassengerService service, ILogger<BookingPassengerController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingPassengerResponse>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all booking-passengers");
                var bookingPassengers = await _service.GetAllAsync();
                return Ok(bookingPassengers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all booking-passengers");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookingPassengerResponse>> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching booking-passenger {Id}", id);
                var bookingPassenger = await _service.GetByIdAsync(id);

                if (bookingPassenger == null)
                {
                    _logger.LogInformation("Booking-passenger {Id} not found", id);
                    return NotFound();
                }

                return Ok(bookingPassenger);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching booking-passenger {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<BookingPassengerResponse>> Create([FromBody] CreateBookingPassengerRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid create booking-passenger request: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Creating booking-passenger");
                var createdBookingPassenger = await _service.CreateAsync(request);
                _logger.LogInformation("Booking-passenger {Id} created", createdBookingPassenger.BookingPassengerId);

                return CreatedAtAction(nameof(GetById),
                    new { id = createdBookingPassenger.BookingPassengerId },
                    createdBookingPassenger);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating booking-passenger");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBookingPassengerRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid update booking-passenger request: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Updating booking-passenger {Id}", request.BookingPassengerId);
                var updated = await _service.UpdateAsync(request);

                if (updated)
                {
                    _logger.LogInformation("Booking-passenger {Id} updated", request.BookingPassengerId);
                    return NoContent();
                }

                _logger.LogInformation("Booking-passenger {Id} not found for update", request.BookingPassengerId);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating booking-passenger {Id}", request.BookingPassengerId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting booking-passenger {Id}", id);
                var deleted = await _service.DeleteAsync(id);

                if (deleted)
                {
                    _logger.LogInformation("Booking-passenger {Id} deleted", id);
                    return NoContent();
                }

                _logger.LogInformation("Booking-passenger {Id} not found for deletion", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting booking-passenger {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }
    }
}