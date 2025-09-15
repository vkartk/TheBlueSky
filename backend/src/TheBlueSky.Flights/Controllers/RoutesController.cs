using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBlueSky.Flights.Services;
using Route = TheBlueSky.Flights.Models.Route;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public RoutesController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Route>>> GetAllRoutes()
        {
            var routes = await _routeService.GetAllRoutesAsync();
            return Ok(routes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Route>> GetRouteById(int id)
        {
            var route = await _routeService.GetRouteByIdAsync(id);

            if (route == null)
            {
                return NotFound();
            }

            return Ok(route);
        }

        [HttpPost]
        public async Task<ActionResult<Route>> CreateRoute(Route route)
        {
            var createdRoute = await _routeService.CreateRouteAsync(route);

            return CreatedAtAction("GetRouteById", new { id = createdRoute.RouteId }, createdRoute);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoute(int id, Route route)
        {
            var success = await _routeService.UpdateRouteAsync(id, route);

            if (!success)
            {
                if (await _routeService.GetRouteByIdAsync(id) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            var success = await _routeService.DeleteRouteAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
