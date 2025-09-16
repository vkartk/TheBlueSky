using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBlueSky.Bookings.DTOs.Requests.Payment;
using TheBlueSky.Bookings.DTOs.Responses.Payment;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _service;
        public PaymentController(IPaymentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentResponse>>> GetAll()
        {
            var payments = await _service.GetAllAsync();

            return Ok(payments);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PaymentResponse>> GetById(int id)
        {
            var payment = await _service.GetByIdAsync(id);

            if (payment == null) return NotFound();

            return Ok(payment);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentResponse>> Create([FromBody] CreatePaymentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var payment = await _service.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = payment.PaymentId }, payment);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePaymentRequest request)
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
