using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheBlueSky.Flights.DTOs.Requests.ScheduleDay;
using TheBlueSky.Flights.DTOs.Responses.ScheduleDay;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User,FlightsOwner")]
    public class ScheduleDayController : ControllerBase
    {
        private readonly IScheduleDayService _scheduleDayService;
        private readonly ILogger<ScheduleDayController> _logger;

        public ScheduleDayController(IScheduleDayService scheduleDayService, ILogger<ScheduleDayController> logger)
        {
            _scheduleDayService = scheduleDayService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleDayResponse>>> GetAllScheduleDays()
        {
            try
            {
                _logger.LogInformation("Fetching all schedule days");
                var days = await _scheduleDayService.GetAllScheduleDaysAsync();
                return Ok(days);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all schedule days");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ScheduleDayResponse>> GetScheduleDayById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching schedule day {Id}", id);
                var day = await _scheduleDayService.GetScheduleDayByIdAsync(id);
                if (day == null) return NotFound();
                return Ok(day);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching schedule day {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult<ScheduleDayResponse>> CreateScheduleDay([FromBody] CreateScheduleDayRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Creating schedule day");
                var createdDay = await _scheduleDayService.CreateScheduleDayAsync(request);
                _logger.LogInformation("Schedule day {Id} created", createdDay.ScheduleDayId);
                return CreatedAtAction(nameof(GetScheduleDayById), new { id = createdDay.ScheduleDayId }, createdDay);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating schedule day");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult> UpdateScheduleDay([FromBody] UpdateScheduleDayRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Updating schedule day {Id}", request.ScheduleDayId);
                var updated = await _scheduleDayService.UpdateScheduleDayAsync(request);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating schedule day {Id}", request.ScheduleDayId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult> DeleteScheduleDayById(int id)
        {
            try
            {
                _logger.LogInformation("Deleting schedule day {Id}", id);
                var deleted = await _scheduleDayService.DeleteScheduleDayAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting schedule day {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }
    }
}
