using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBlueSky.Bookings.DTOs.Requests.BookingCancellation;
using TheBlueSky.Bookings.DTOs.Responses.BookingCancellation;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingCancellationController : ControllerBase
    {
        private readonly IBookingCancellationService _service;
        private readonly ILogger<BookingCancellationController> _logger;

        public BookingCancellationController(
            IBookingCancellationService service,
            ILogger<BookingCancellationController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingCancellationResponse>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all booking cancellations");
                var items = await _service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all booking cancellations");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookingCancellationResponse>> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching booking cancellation {Id}", id);
                var item = await _service.GetByIdAsync(id);

                if (item == null)
                {
                    _logger.LogInformation("Booking cancellation {Id} not found", id);
                    return NotFound();
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching booking cancellation {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<BookingCancellationResponse>> Create([FromBody] CreateBookingCancellationRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid create request for booking cancellation: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Creating booking cancellation for BookingId {BookingId} by User {UserId}",
                    request.BookingId, request.CancelledByUserId);

                var created = await _service.CreateAsync(request);

                _logger.LogInformation("Booking cancellation {Id} created", created.BookingCancellationId);
                return CreatedAtAction(nameof(GetById), new { id = created.BookingCancellationId }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating booking cancellation for BookingId {BookingId}", request.BookingId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBookingCancellationRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid update request for booking cancellation: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Updating booking cancellation {Id}", request.BookingCancellationId);
                var updated = await _service.UpdateAsync(request);

                if (updated)
                {
                    _logger.LogInformation("Booking cancellation {Id} updated", request.BookingCancellationId);
                    return NoContent();
                }

                _logger.LogInformation("Booking cancellation {Id} not found for update", request.BookingCancellationId);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating booking cancellation {Id}", request.BookingCancellationId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting booking cancellation {Id}", id);
                var deleted = await _service.DeleteAsync(id);

                if (deleted)
                {
                    _logger.LogInformation("Booking cancellation {Id} deleted", id);
                    return NoContent();
                }

                _logger.LogInformation("Booking cancellation {Id} not found for deletion", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting booking cancellation {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }
    }
}