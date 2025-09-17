using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheBlueSky.Bookings.DTOs.Requests.MealPreference;
using TheBlueSky.Bookings.DTOs.Responses.MealPreference;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealPreferenceController : ControllerBase
    {
        private readonly IMealPreferenceService _service;
        private readonly ILogger<MealPreferenceController> _logger;

        public MealPreferenceController(IMealPreferenceService service, ILogger<MealPreferenceController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealPreferenceResponse>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all meal preferences");
                var items = await _service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all meal preferences");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MealPreferenceResponse>> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching meal preference {Id}", id);
                var item = await _service.GetByIdAsync(id);

                if (item == null)
                {
                    _logger.LogInformation("Meal preference {Id} not found", id);
                    return NotFound();
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching meal preference {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<MealPreferenceResponse>> Create([FromBody] CreateMealPreferenceRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid create meal preference request: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Creating meal preference");
                var created = await _service.CreateAsync(request);
                _logger.LogInformation("Meal preference {Id} created", created.MealPreferenceId);

                return CreatedAtAction(nameof(GetById), new { id = created.MealPreferenceId }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating meal preference");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateMealPreferenceRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid update meal preference request: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Updating meal preference {Id}", request.MealPreferenceId);
                var updated = await _service.UpdateAsync(request);

                if (updated)
                {
                    _logger.LogInformation("Meal preference {Id} updated", request.MealPreferenceId);
                    return NoContent();
                }

                _logger.LogInformation("Meal preference {Id} not found for update", request.MealPreferenceId);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating meal preference {Id}", request.MealPreferenceId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting meal preference {Id}", id);
                var deleted = await _service.DeleteAsync(id);

                if (deleted)
                {
                    _logger.LogInformation("Meal preference {Id} deleted", id);
                    return NoContent();
                }

                _logger.LogInformation("Meal preference {Id} not found for deletion", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting meal preference {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }
    }
}