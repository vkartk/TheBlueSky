using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheBlueSky.Flights.DTOs.Requests.Route;
using TheBlueSky.Flights.DTOs.Responses.Route;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User,FlightsOwner")]
    public class RoutesController : ControllerBase
    {
        private readonly IRouteService _routeService;
        private readonly ILogger<RoutesController> _logger;

        public RoutesController(IRouteService routeService, ILogger<RoutesController> logger)
        {
            _routeService = routeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RouteResponse>>> GetAllRoutes()
        {
            try
            {
                _logger.LogInformation("Fetching all routes");
                var routes = await _routeService.GetAllRoutesAsync();
                return Ok(routes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all routes");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RouteResponse>> GetRouteById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching route {Id}", id);
                var route = await _routeService.GetRouteByIdAsync(id);
                if (route == null) return NotFound();
                return Ok(route);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching route {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult<RouteResponse>> CreateRoute([FromBody] CreateRouteRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Creating route");
                var createdRoute = await _routeService.CreateRouteAsync(request);
                _logger.LogInformation("Route {Id} created", createdRoute.RouteId);
                return CreatedAtAction(nameof(GetRouteById), new { id = createdRoute.RouteId }, createdRoute);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating route");
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult> UpdateRoute([FromBody] UpdateRouteRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Updating route {Id}", request.RouteId);
                var updated = await _routeService.UpdateRouteAsync(request);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating route {Id}", request.RouteId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,FlightsOwner")]
        public async Task<ActionResult> DeleteRouteById(int id)
        {
            try
            {
                _logger.LogInformation("Deleting route {Id}", id);
                var deleted = await _routeService.DeleteRouteAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting route {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
            }
        }
    }
}