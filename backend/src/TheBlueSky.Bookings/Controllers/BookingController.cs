using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheBlueSky.Bookings.DTOs.Requests.Booking;
using TheBlueSky.Bookings.DTOs.Responses.Booking;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;
        private readonly ILogger<BookingController> _logger;

        public BookingController(IBookingService service, ILogger<BookingController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingResponse>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all bookings");
                var bookings = await _service.GetAllAsync();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all bookings");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookingResponse>> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching booking {Id}", id);
                var booking = await _service.GetByIdAsync(id);

                if (booking == null)
                {
                    _logger.LogInformation("Booking {Id} not found", id);
                    return NotFound();
                }

                return Ok(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching booking {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<BookingResponse>> Create([FromBody] CreateBookingRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid create booking request: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Creating booking for Flight {FlightId} by User {UserId}",
                    request.FlightId, request.UserId);

                var booking = await _service.CreateAsync(request);

                _logger.LogInformation("Booking {BookingId} created", booking.BookingId);
                return CreatedAtAction(nameof(GetById), new { id = booking.BookingId }, booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating booking for Flight {FlightId}", request.FlightId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBookingRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid update booking request: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Updating booking {Id}", request.BookingId);
                var updated = await _service.UpdateAsync(request);

                if (updated)
                {
                    _logger.LogInformation("Booking {Id} updated", request.BookingId);
                    return NoContent();
                }

                _logger.LogInformation("Booking {Id} not found for update", request.BookingId);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating booking {Id}", request.BookingId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting booking {Id}", id);
                var deleted = await _service.DeleteAsync(id);

                if (deleted)
                {
                    _logger.LogInformation("Booking {Id} deleted", id);
                    return NoContent();
                }

                _logger.LogInformation("Booking {Id} not found for deletion", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting booking {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }
    }
}