using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBlueSky.Flights.DTOs.Requests.ScheduleDay;
using TheBlueSky.Flights.DTOs.Responses.ScheduleDay;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleDayController : ControllerBase
    {
        private readonly IScheduleDayService _scheduleDayService;

        public ScheduleDayController(IScheduleDayService scheduleDayService)
        {
            _scheduleDayService = scheduleDayService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleDayResponse>>> GetAllScheduleDays()
        {
            var days = await _scheduleDayService.GetAllScheduleDaysAsync();
            return Ok(days);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleDayResponse>> GetScheduleDayById(int id)
        {
            var day = await _scheduleDayService.GetScheduleDayByIdAsync(id);
            if (day == null)
            {
                return NotFound();
            }
            return Ok(day);
        }

        [HttpPost]
        public async Task<ActionResult<ScheduleDayResponse>> CreateScheduleDay([FromBody] CreateScheduleDayRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdDay = await _scheduleDayService.CreateScheduleDayAsync(request);
            return CreatedAtAction(nameof(GetScheduleDayById), new { id = createdDay.ScheduleDayId }, createdDay);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateScheduleDay([FromBody] UpdateScheduleDayRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await _scheduleDayService.UpdateScheduleDayAsync(request);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteScheduleDayById(int id)
        {
            var deleted = await _scheduleDayService.DeleteScheduleDayAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
