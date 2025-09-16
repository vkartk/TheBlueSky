using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBlueSky.Flights.DTOs.Requests.Airport;
using TheBlueSky.Flights.DTOs.Responses.Airport;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportsController : ControllerBase
    {
        private readonly IAirportService _airportService;

        public AirportsController(IAirportService airportService)
        {
            _airportService = airportService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AirportResponse>>> GetAllAirports()
        {
            var airports = await _airportService.GetAllAirportsAsync();
            return Ok(airports);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AirportResponse>> GetAirportById(int id)
        {
            var airport = await _airportService.GetAirportByIdAsync(id);
            if (airport == null)
            {
                return NotFound();
            }
            return Ok(airport);
        }

        [HttpPost]
        public async Task<ActionResult<AirportResponse>> CreateAirport([FromBody] CreateAirportRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdAirport = await _airportService.CreateAirportAsync(request);
            return CreatedAtAction(nameof(GetAirportById), new { id = createdAirport.AirportId }, createdAirport);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAirport([FromBody] UpdateAirportRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await _airportService.UpdateAirportAsync(request);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAirportById(int id)
        {
            var deleted = await _airportService.DeleteAirportAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
