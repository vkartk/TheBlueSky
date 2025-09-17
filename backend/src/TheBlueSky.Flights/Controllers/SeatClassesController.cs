using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheBlueSky.Flights.DTOs.Requests.SeatClass;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User,FlightsOwner")]
    public class SeatClassesController : ControllerBase
    {
        private readonly ISeatClassService _seatClassService;
        private readonly ILogger<SeatClassesController> _logger;

        public SeatClassesController(ISeatClassService seatClassService, ILogger<SeatClassesController> logger)
        {
            _seatClassService = seatClassService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeatClassDto>>> GetAllSeatClasses()
        {
            try
            {
                _logger.LogInformation("Fetching all seat classes");
                var seatClasses = await _seatClassService.GetAllSeatClassesAsync();
                return Ok(seatClasses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all seat classes");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SeatClassDto>> GetSeatClassById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching seat class {Id}", id);
                var seatClass = await _seatClassService.GetSeatClassByIdAsync(id);
                if (seatClass == null) return NotFound();
                return Ok(seatClass);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching seat class {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult<SeatClassDto>> CreateSeatClass([FromBody] CreateSeatClassRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Creating seat class");
                var createdSeatClass = await _seatClassService.CreateSeatClassAsync(request);
                _logger.LogInformation("Seat class {Id} created", createdSeatClass.SeatClassId);
                return CreatedAtAction(nameof(GetSeatClassById), new { id = createdSeatClass.SeatClassId }, createdSeatClass);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating seat class");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<IActionResult> UpdateSeatClass(int id, [FromBody] UpdateSeatClassRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Updating seat class {Id}", id);
                var success = await _seatClassService.UpdateSeatClassAsync(id, request);
                if (!success)
                {
                    if (await _seatClassService.GetSeatClassByIdAsync(id) == null) return NotFound();
                    return BadRequest();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating seat class {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<IActionResult> DeleteSeatClass(int id)
        {
            try
            {
                _logger.LogInformation("Deleting seat class {Id}", id);
                var success = await _seatClassService.DeleteSeatClassAsync(id);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting seat class {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }
    }
}