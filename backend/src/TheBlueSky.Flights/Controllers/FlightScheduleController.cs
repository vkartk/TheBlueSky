using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheBlueSky.Flights.DTOs.Requests.Flight;
using TheBlueSky.Flights.DTOs.Requests.FlightSchedule;
using TheBlueSky.Flights.DTOs.Responses.Flight;
using TheBlueSky.Flights.DTOs.Responses.FlightSchedule;
using TheBlueSky.Flights.DTOs.Responses.ScheduleDay;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User,FlightsOwner")]
    public class FlightScheduleController : ControllerBase
    {
        private readonly IFlightScheduleService _flightScheduleService;
        private readonly ILogger<FlightScheduleController> _logger;

        public FlightScheduleController(IFlightScheduleService flightScheduleService, ILogger<FlightScheduleController> logger)
        {
            _flightScheduleService = flightScheduleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightScheduleResponse>>> GetAllFlightSchedules()
        {
            try
            {
                _logger.LogInformation("Fetching all flight schedules");
                var schedules = await _flightScheduleService.GetAllFlightSchedulesAsync();
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all flight schedules");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<FlightScheduleResponse>> GetFlightScheduleById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching flight schedule {Id}", id);
                var schedule = await _flightScheduleService.GetFlightScheduleByIdAsync(id);
                if (schedule == null) return NotFound();
                return Ok(schedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching flight schedule {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult<FlightScheduleResponse>> CreateFlightSchedule([FromBody] CreateFlightScheduleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Creating flight schedule");
                var createdSchedule = await _flightScheduleService.CreateFlightScheduleAsync(request);
                _logger.LogInformation("Flight schedule {Id} created", createdSchedule.FlightScheduleId);
                return CreatedAtAction(nameof(GetFlightScheduleById), new { id = createdSchedule.FlightScheduleId }, createdSchedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating flight schedule");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult> UpdateFlightSchedule([FromBody] UpdateFlightScheduleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Updating flight schedule {Id}", request.FlightScheduleId);
                var updated = await _flightScheduleService.UpdateFlightScheduleAsync(request);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating flight schedule {Id}", request.FlightScheduleId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult> DeleteFlightScheduleById(int id)
        {
            try
            {
                _logger.LogInformation("Deleting flight schedule {Id}", id);
                var deleted = await _flightScheduleService.DeleteFlightScheduleAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting flight schedule {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpGet("{id:int}/flights")]
        public async Task<ActionResult<IEnumerable<FlightResponse>>> GetFlightsForSchedule(int id)
        {
            try
            {
                var flights = await _flightScheduleService.GetFlightsForScheduleAsync(id);
                if (flights == null || !flights.Any())
                {
                    _logger.LogInformation("No flights found for schedule {Id}", id);
                    return NotFound();
                }
                return Ok(flights);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching flights for schedule {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPost("{id:int}/scheduleDays")]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult<IEnumerable<ScheduleDayResponse>>> UpdateScheduleDays(int id, [FromBody] IEnumerable<string> daysOfWeek)
        {
            if (daysOfWeek == null || !daysOfWeek.Any())
            {
                return BadRequest("Days of week cannot be null or empty.");
            }

            try
            {
                var updatedScheduleDays = await _flightScheduleService.UpdateScheduleDaysAsync(id, daysOfWeek);
                if (updatedScheduleDays == null || !updatedScheduleDays.Any())
                {
                    _logger.LogInformation("No schedule days found or updated for flight schedule {Id}", id);
                    return NotFound();
                }
                return Ok(updatedScheduleDays);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating schedule days for flight schedule {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPost("{id:int}/generate")]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<IActionResult> GenerateFlights(int id, [FromBody] GenerateFlightsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var count = await _flightScheduleService.GenerateFlightsAsync(id, request.StartDate, request.EndDate);
            return Ok(new { message = $"Successfully generated {count} flights.", count });
        }


    }
}
