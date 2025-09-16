using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public BookingPassengerController(IBookingPassengerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingPassengerResponse>>> GetAll()
        {
            var bookingPassengers = await _service.GetAllAsync();
            return Ok(bookingPassengers);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookingPassengerResponse>> GetById(int id)
        {
            var bookingPassenger = await _service.GetByIdAsync(id);
            if (bookingPassenger == null) return NotFound();
            return Ok(bookingPassenger);
        }

        [HttpPost]
        public async Task<ActionResult<BookingPassengerResponse>> Create([FromBody] CreateBookingPassengerRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var createdBookingPassenger = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdBookingPassenger.BookingPassengerId }, createdBookingPassenger);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBookingPassengerRequest request)
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
