using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheBlueSky.Bookings.DTOs.Requests.Passenger;
using TheBlueSky.Bookings.DTOs.Responses.Passenger;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly IPassengerService _service;
        private readonly ILogger<PassengerController> _logger;

        public PassengerController(IPassengerService service, ILogger<PassengerController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PassengerResponse>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all passengers");
                var items = await _service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all passengers");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PassengerResponse>> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching passenger {Id}", id);
                var item = await _service.GetByIdAsync(id);

                if (item == null)
                {
                    _logger.LogInformation("Passenger {Id} not found", id);
                    return NotFound();
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching passenger {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PassengerResponse>> Create([FromBody] CreatePassengerRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid create passenger request: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Creating passenger");
                var created = await _service.CreateAsync(request);
                _logger.LogInformation("Passenger {Id} created", created.PassengerId);

                return CreatedAtAction(nameof(GetById), new { id = created.PassengerId }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating passenger");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePassengerRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid update passenger request: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Updating passenger {Id}", request.PassengerId);
                var updated = await _service.UpdateAsync(request);

                if (updated)
                {
                    _logger.LogInformation("Passenger {Id} updated", request.PassengerId);
                    return NoContent();
                }

                _logger.LogInformation("Passenger {Id} not found for update", request.PassengerId);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating passenger {Id}", request.PassengerId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting passenger {Id}", id);
                var deleted = await _service.DeleteAsync(id);

                if (deleted)
                {
                    _logger.LogInformation("Passenger {Id} deleted", id);
                    return NoContent();
                }

                _logger.LogInformation("Passenger {Id} not found for deletion", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting passenger {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }
    }
}