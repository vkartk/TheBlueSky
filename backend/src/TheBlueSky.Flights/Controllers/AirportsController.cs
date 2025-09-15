using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IEnumerable<Airport>>> GetAirports()
        {
            var airports = await _airportService.GetAllAirportsAsync();
            return Ok(airports);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Airport>> GetAirportById(int id)
        {
            var airport = await _airportService.GetAirportByIdAsync(id);

            if (airport == null)
            {
                return NotFound();
            }

            return Ok(airport);
        }

        [HttpPost]
        public async Task<ActionResult<Airport>> CreateAirport(Airport airport)
        {
            var createdAirport = await _airportService.CreateAirportAsync(airport);

            return CreatedAtAction("GetAirportById", new { id = createdAirport.AirportId }, createdAirport);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAirport(int id, Airport airport)
        {
            var success = await _airportService.UpdateAirportAsync(id, airport);

            if (!success)
            {
                if (await _airportService.GetAirportByIdAsync(id) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAirport(int id)
        {
            var success = await _airportService.DeleteAirportAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
