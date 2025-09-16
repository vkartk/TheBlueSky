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
        public BookingCancellationController(IBookingCancellationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingCancellationResponse>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookingCancellationResponse>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);

            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<BookingCancellationResponse>> Create([FromBody] CreateBookingCancellationRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _service.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = created.BookingCancellationId }, created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBookingCancellationRequest request)
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
