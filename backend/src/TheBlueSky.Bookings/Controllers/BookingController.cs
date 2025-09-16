using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public BookingController(IBookingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingResponse>>> GetAll()
        {
            var bookings = await _service.GetAllAsync();
            return Ok(bookings);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookingResponse>> GetById(int id)
        {
            var booking = await _service.GetByIdAsync(id);

            if (booking == null) return NotFound();

            return Ok(booking);
        }

        [HttpPost]
        public async Task<ActionResult<BookingResponse>> Create([FromBody] CreateBookingRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var booking = await _service.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = booking.BookingId }, booking);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBookingRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(request);

            if (updated) return NoContent();

            return NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (deleted) return NoContent();

            return NotFound();
        }
    }
}
