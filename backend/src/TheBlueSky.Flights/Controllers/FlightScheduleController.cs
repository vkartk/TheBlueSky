using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBlueSky.Flights.DTOs.Requests.FlightSchedule;
using TheBlueSky.Flights.DTOs.Responses.FlightSchedule;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightScheduleController : ControllerBase
    {
        private readonly IFlightScheduleService _flightScheduleService;

        public FlightScheduleController(IFlightScheduleService flightScheduleService)
        {
            _flightScheduleService = flightScheduleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightScheduleResponse>>> GetAllFlightSchedules()
        {
            var schedules = await _flightScheduleService.GetAllFlightSchedulesAsync();
            return Ok(schedules);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FlightScheduleResponse>> GetFlightScheduleById(int id)
        {
            var schedule = await _flightScheduleService.GetFlightScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }

        [HttpPost]
        public async Task<ActionResult<FlightScheduleResponse>> CreateFlightSchedule([FromBody] CreateFlightScheduleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdSchedule = await _flightScheduleService.CreateFlightScheduleAsync(request);
            return CreatedAtAction(nameof(GetFlightScheduleById), new { id = createdSchedule.FlightScheduleId }, createdSchedule);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateFlightSchedule([FromBody] UpdateFlightScheduleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await _flightScheduleService.UpdateFlightScheduleAsync(request);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFlightScheduleById(int id)
        {
            var deleted = await _flightScheduleService.DeleteFlightScheduleAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
