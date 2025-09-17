using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheBlueSky.Flights.DTOs.Requests.Airport;
using TheBlueSky.Flights.DTOs.Responses.Airport;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User,FlightsOwner")]
    public class AirportsController : ControllerBase
    {
        private readonly IAirportService _airportService;
        private readonly ILogger<AirportsController> _logger;

        public AirportsController(IAirportService airportService, ILogger<AirportsController> logger)
        {
            _airportService = airportService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AirportResponse>>> GetAllAirports()
        {
            try
            {
                _logger.LogInformation("Fetching all airports");
                var airports = await _airportService.GetAllAirportsAsync();
                return Ok(airports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all airports");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AirportResponse>> GetAirportById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching airport {Id}", id);
                var airport = await _airportService.GetAirportByIdAsync(id);
                if (airport == null) return NotFound();
                return Ok(airport);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching airport {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult<AirportResponse>> CreateAirport([FromBody] CreateAirportRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Creating airport");
                var createdAirport = await _airportService.CreateAirportAsync(request);
                _logger.LogInformation("Airport {Id} created", createdAirport.AirportId);
                return CreatedAtAction(nameof(GetAirportById), new { id = createdAirport.AirportId }, createdAirport);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating airport");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult> UpdateAirport([FromBody] UpdateAirportRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Updating airport {Id}", request.AirportId);
                var updated = await _airportService.UpdateAirportAsync(request);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating airport {Id}", request.AirportId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult> DeleteAirportById(int id)
        {
            try
            {
                _logger.LogInformation("Deleting airport {Id}", id);
                var deleted = await _airportService.DeleteAirportAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting airport {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }
    }
}