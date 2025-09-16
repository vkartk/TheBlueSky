using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBlueSky.Flights.DTOs.Requests.FlightSeatStatus;
using TheBlueSky.Flights.DTOs.Responses.FlightSeatStatus;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightSeatStatusController : ControllerBase
    {
        private readonly IFlightSeatStatusService _flightSeatStatusService;

        public FlightSeatStatusController(IFlightSeatStatusService flightSeatStatusService)
        {
            _flightSeatStatusService = flightSeatStatusService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightSeatStatusResponse>>> GetAllFlightSeatStatuses()
        {
            var statuses = await _flightSeatStatusService.GetAllFlightSeatStatusesAsync();
            return Ok(statuses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FlightSeatStatusResponse>> GetFlightSeatStatusById(int id)
        {
            var status = await _flightSeatStatusService.GetFlightSeatStatusByIdAsync(id);
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }

        [HttpPost]
        public async Task<ActionResult<FlightSeatStatusResponse>> CreateFlightSeatStatus([FromBody] CreateFlightSeatStatusRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdStatus = await _flightSeatStatusService.CreateFlightSeatStatusAsync(request);
            return CreatedAtAction(nameof(GetFlightSeatStatusById), new { id = createdStatus.FlightSeatStatusId }, createdStatus);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateFlightSeatStatus([FromBody] UpdateFlightSeatStatusRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await _flightSeatStatusService.UpdateFlightSeatStatusAsync(request);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFlightSeatStatusById(int id)
        {
            var deleted = await _flightSeatStatusService.DeleteFlightSeatStatusAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
