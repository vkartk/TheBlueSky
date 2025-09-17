using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheBlueSky.Flights.DTOs.Requests.Aircraft;
using TheBlueSky.Flights.DTOs.Responses.Aircraft;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User,FlightsOwner")]
    public class AircraftController : ControllerBase
    {
        private readonly IAircraftService _aircraftService;
        private readonly ILogger<AircraftController> _logger;

        public AircraftController(IAircraftService aircraftService, ILogger<AircraftController> logger)
        {
            _aircraftService = aircraftService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AircraftResponse>>> GetAllAircrafts()
        {
            try
            {
                _logger.LogInformation("Fetching all aircrafts");
                var aircrafts = await _aircraftService.GetAllAircraftsAsync();
                return Ok(aircrafts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all aircrafts");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AircraftResponse>> GetAircraftById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching aircraft {Id}", id);
                var aircraft = await _aircraftService.GetAircraftByIdAsync(id);
                if (aircraft == null)
                {
                    _logger.LogInformation("Aircraft {Id} not found", id);
                    return NotFound();
                }
                return Ok(aircraft);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching aircraft {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult<AircraftResponse>> CreateAircraft([FromBody] CreateAircraftRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid create aircraft request: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Creating aircraft");
                var createdAircraft = await _aircraftService.CreateAircraftAsync(request);
                _logger.LogInformation("Aircraft {Id} created", createdAircraft.AircraftId);
                return CreatedAtAction(nameof(GetAircraftById), new { id = createdAircraft.AircraftId }, createdAircraft);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating aircraft");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult> UpdateAircraft([FromBody] UpdateAircraftRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid update aircraft request: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Updating aircraft {Id}", request.AircraftId);
                var updated = await _aircraftService.UpdateAircraftAsync(request);
                if (!updated)
                {
                    _logger.LogInformation("Aircraft {Id} not found for update", request.AircraftId);
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating aircraft {Id}", request.AircraftId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult> DeleteAircraftById(int id)
        {
            try
            {
                _logger.LogInformation("Deleting aircraft {Id}", id);
                var deleted = await _aircraftService.DeleteAircraftAsync(id);
                if (!deleted)
                {
                    _logger.LogInformation("Aircraft {Id} not found for deletion", id);
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting aircraft {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }
    }
}