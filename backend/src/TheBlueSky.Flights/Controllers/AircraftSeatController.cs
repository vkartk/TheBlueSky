using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBlueSky.Flights.DTOs.Requests.AircraftSeat;
using TheBlueSky.Flights.DTOs.Responses.AircraftSeat;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftSeatController : ControllerBase
    {
        private readonly IAircraftSeatService _aircraftSeatService;

        public AircraftSeatController(IAircraftSeatService aircraftSeatService)
        {
            _aircraftSeatService = aircraftSeatService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AircraftSeatResponse>>> GetAllAircraftSeats()
        {
            var aircraftSeats = await _aircraftSeatService.GetAllAircraftSeatsAsync();
            return Ok(aircraftSeats);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AircraftSeatResponse>> GetAircraftSeatById(int id)
        {
            var aircraftSeat = await _aircraftSeatService.GetAircraftSeatByIdAsync(id);
            if (aircraftSeat == null)
            {
                return NotFound();
            }
            return Ok(aircraftSeat);
        }

        [HttpPost]
        public async Task<ActionResult<AircraftSeatResponse>> CreateAircraftSeat([FromBody] CreateAircraftSeatRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdSeat = await _aircraftSeatService.CreateAircraftSeatAsync(request);
            return CreatedAtAction(nameof(GetAircraftSeatById), new { id = createdSeat.AircraftSeatId }, createdSeat);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAircraftSeat([FromBody] UpdateAircraftSeatRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await _aircraftSeatService.UpdateAircraftSeatAsync(request);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAircraftSeatById(int id)
        {
            var deleted = await _aircraftSeatService.DeleteAircraftSeatAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
