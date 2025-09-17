using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheBlueSky.Flights.DTOs.Requests.AircraftSeat;
using TheBlueSky.Flights.DTOs.Responses.AircraftSeat;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User,FlightsOwner")]
    public class AircraftSeatController : ControllerBase
    {
        private readonly IAircraftSeatService _aircraftSeatService;
        private readonly ILogger<AircraftSeatController> _logger;

        public AircraftSeatController(IAircraftSeatService aircraftSeatService, ILogger<AircraftSeatController> logger)
        {
            _aircraftSeatService = aircraftSeatService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AircraftSeatResponse>>> GetAllAircraftSeats()
        {
            try
            {
                _logger.LogInformation("Fetching all aircraft seats");
                var aircraftSeats = await _aircraftSeatService.GetAllAircraftSeatsAsync();
                return Ok(aircraftSeats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all aircraft seats");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AircraftSeatResponse>> GetAircraftSeatById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching aircraft seat {Id}", id);
                var aircraftSeat = await _aircraftSeatService.GetAircraftSeatByIdAsync(id);
                if (aircraftSeat == null) return NotFound();
                return Ok(aircraftSeat);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching aircraft seat {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult<AircraftSeatResponse>> CreateAircraftSeat([FromBody] CreateAircraftSeatRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Creating aircraft seat");
                var createdSeat = await _aircraftSeatService.CreateAircraftSeatAsync(request);
                _logger.LogInformation("Aircraft seat {Id} created", createdSeat.AircraftSeatId);
                return CreatedAtAction(nameof(GetAircraftSeatById), new { id = createdSeat.AircraftSeatId }, createdSeat);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating aircraft seat");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult> UpdateAircraftSeat([FromBody] UpdateAircraftSeatRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Updating aircraft seat {Id}", request.AircraftSeatId);
                var updated = await _aircraftSeatService.UpdateAircraftSeatAsync(request);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating aircraft seat {Id}", request.AircraftSeatId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult> DeleteAircraftSeatById(int id)
        {
            try
            {
                _logger.LogInformation("Deleting aircraft seat {Id}", id);
                var deleted = await _aircraftSeatService.DeleteAircraftSeatAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting aircraft seat {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }
    }
}