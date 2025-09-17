using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheBlueSky.Flights.DTOs.Requests.FlightSeatStatus;
using TheBlueSky.Flights.DTOs.Responses.FlightSeatStatus;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User,FlightsOwner")]
    public class FlightSeatStatusController : ControllerBase
    {
        private readonly IFlightSeatStatusService _flightSeatStatusService;
        private readonly ILogger<FlightSeatStatusController> _logger;

        public FlightSeatStatusController(IFlightSeatStatusService flightSeatStatusService, ILogger<FlightSeatStatusController> logger)
        {
            _flightSeatStatusService = flightSeatStatusService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightSeatStatusResponse>>> GetAllFlightSeatStatuses()
        {
            try
            {
                _logger.LogInformation("Fetching all flight seat statuses");
                var statuses = await _flightSeatStatusService.GetAllFlightSeatStatusesAsync();
                return Ok(statuses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all flight seat statuses");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<FlightSeatStatusResponse>> GetFlightSeatStatusById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching flight seat status {Id}", id);
                var status = await _flightSeatStatusService.GetFlightSeatStatusByIdAsync(id);
                if (status == null) return NotFound();
                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching flight seat status {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult<FlightSeatStatusResponse>> CreateFlightSeatStatus([FromBody] CreateFlightSeatStatusRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Creating flight seat status");
                var createdStatus = await _flightSeatStatusService.CreateFlightSeatStatusAsync(request);
                _logger.LogInformation("Flight seat status {Id} created", createdStatus.FlightSeatStatusId);
                return CreatedAtAction(nameof(GetFlightSeatStatusById), new { id = createdStatus.FlightSeatStatusId }, createdStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating flight seat status");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult> UpdateFlightSeatStatus([FromBody] UpdateFlightSeatStatusRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Updating flight seat status {Id}", request.FlightSeatStatusId);
                var updated = await _flightSeatStatusService.UpdateFlightSeatStatusAsync(request);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating flight seat status {Id}", request.FlightSeatStatusId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult> DeleteFlightSeatStatusById(int id)
        {
            try
            {
                _logger.LogInformation("Deleting flight seat status {Id}", id);
                var deleted = await _flightSeatStatusService.DeleteFlightSeatStatusAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting flight seat status {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }
    }
}
