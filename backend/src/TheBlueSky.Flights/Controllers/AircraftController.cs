using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBlueSky.Flights.DTOs.Requests.Aircraft;
using TheBlueSky.Flights.DTOs.Responses.Aircraft;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftController : ControllerBase
    {
        private readonly IAircraftService _aircraftService;

        public AircraftController(IAircraftService aircraftService)
        {
            _aircraftService = aircraftService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AircraftResponse>>> GetAllAircrafts()
        {
            var aircrafts = await _aircraftService.GetAllAircraftsAsync();
            return Ok(aircrafts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AircraftResponse>> GetAircraftById(int id)
        {
            var aircraft = await _aircraftService.GetAircraftByIdAsync(id);
            if (aircraft == null)
            {
                return NotFound();
            }
            return Ok(aircraft);
        }

        [HttpPost]
        public async Task<ActionResult<AircraftResponse>> CreateAircraft([FromBody] CreateAircraftRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdAircraft = await _aircraftService.CreateAircraftAsync(request);
            return CreatedAtAction(nameof(GetAircraftById), new { id = createdAircraft.AircraftId }, createdAircraft);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAircraft([FromBody] UpdateAircraftRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await _aircraftService.UpdateAircraftAsync(request);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAircraftById(int id)
        {
            var deleted = await _aircraftService.DeleteAircraftAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
