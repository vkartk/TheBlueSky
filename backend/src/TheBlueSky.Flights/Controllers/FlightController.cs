using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBlueSky.Flights.DTOs.Requests.Flight;
using TheBlueSky.Flights.DTOs.Responses.Flight;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightResponse>>> GetAllFlights()
        {
            var flights = await _flightService.GetAllFlightsAsync();
            return Ok(flights);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FlightResponse>> GetFlightById(int id)
        {
            var flight = await _flightService.GetFlightByIdAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }

        [HttpPost]
        public async Task<ActionResult<FlightResponse>> CreateFlight([FromBody] CreateFlightRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdFlight = await _flightService.CreateFlightAsync(request);
            return CreatedAtAction(nameof(GetFlightById), new { id = createdFlight.FlightId }, createdFlight);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateFlight([FromBody] UpdateFlightRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await _flightService.UpdateFlightAsync(request);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFlightById(int id)
        {
            var deleted = await _flightService.DeleteFlightAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
