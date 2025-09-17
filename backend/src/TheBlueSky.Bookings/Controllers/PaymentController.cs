using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService service, ILogger<PaymentController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentResponse>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all payments");
                var payments = await _service.GetAllAsync();
                return Ok(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all payments");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PaymentResponse>> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching payment {Id}", id);
                var payment = await _service.GetByIdAsync(id);

                if (payment == null)
                {
                    _logger.LogInformation("Payment {Id} not found", id);
                    return NotFound();
                }

                return Ok(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching payment {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PaymentResponse>> Create([FromBody] CreatePaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid create payment request: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Creating payment");
                var payment = await _service.CreateAsync(request);
                _logger.LogInformation("Payment {Id} created", payment.PaymentId);

                return CreatedAtAction(nameof(GetById), new { id = payment.PaymentId }, payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating payment");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid update payment request: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Updating payment");
                var updated = await _service.UpdateAsync(request);

                if (updated)
                {
                    _logger.LogInformation("Payment updated");
                    return NoContent();
                }

                _logger.LogInformation("Payment not found for update");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating payment");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting payment {Id}", id);
                var deleted = await _service.DeleteAsync(id);

                if (deleted)
                {
                    _logger.LogInformation("Payment {Id} deleted", id);
                    return NoContent();
                }

                _logger.LogInformation("Payment {Id} not found for deletion", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting payment {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }
    }
}