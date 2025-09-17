using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheBlueSky.Flights.DTOs.Requests.Flight;
using TheBlueSky.Flights.DTOs.Responses.Flight;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User,FlightsOwner")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly ILogger<FlightController> _logger;

        public FlightController(IFlightService flightService, ILogger<FlightController> logger)
        {
            _flightService = flightService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightResponse>>> GetAllFlights()
        {
            try
            {
                _logger.LogInformation("Fetching all flights");
                var flights = await _flightService.GetAllFlightsAsync();
                return Ok(flights);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all flights");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<FlightResponse>> GetFlightById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching flight {Id}", id);
                var flight = await _flightService.GetFlightByIdAsync(id);
                if (flight == null) return NotFound();
                return Ok(flight);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching flight {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult<FlightResponse>> CreateFlight([FromBody] CreateFlightRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Creating flight");
                var createdFlight = await _flightService.CreateFlightAsync(request);
                _logger.LogInformation("Flight {Id} created", createdFlight.FlightId);
                return CreatedAtAction(nameof(GetFlightById), new { id = createdFlight.FlightId }, createdFlight);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating flight");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult> UpdateFlight([FromBody] UpdateFlightRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Updating flight {Id}", request.FlightId);
                var updated = await _flightService.UpdateFlightAsync(request);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating flight {Id}", request.FlightId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult> DeleteFlightById(int id)
        {
            try
            {
                _logger.LogInformation("Deleting flight {Id}", id);
                var deleted = await _flightService.DeleteFlightAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting flight {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }
    }
}