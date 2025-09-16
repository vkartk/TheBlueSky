using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBlueSky.Flights.DTOs.Requests.SeatClass;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatClassesController : ControllerBase
    {
        private readonly ISeatClassService _seatClassService;

        public SeatClassesController(ISeatClassService seatClassService)
        {
            _seatClassService = seatClassService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeatClassDto>>> GetAllSeatClasses()
        {
            var seatClasses = await _seatClassService.GetAllSeatClassesAsync();
            return Ok(seatClasses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SeatClassDto>> GetSeatClassById(int id)
        {
            var seatClass = await _seatClassService.GetSeatClassByIdAsync(id);

            if (seatClass == null)
            {
                return NotFound();
            }

            return Ok(seatClass);
        }

        [HttpPost]
        public async Task<ActionResult<SeatClassDto>> CreateSeatClass([FromBody] CreateSeatClassRequest request)
        {
            var createdSeatClass = await _seatClassService.CreateSeatClassAsync(request);

            return CreatedAtAction("GetSeatClassById", new { id = createdSeatClass.SeatClassId }, createdSeatClass);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeatClass(int id, [FromBody] UpdateSeatClassRequest request)
        {
            var success = await _seatClassService.UpdateSeatClassAsync(id, request);

            if (!success)
            {
                if (await _seatClassService.GetSeatClassByIdAsync(id) == null)
                {
                    return NotFound();
                }
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeatClass(int id)
        {
            var success = await _seatClassService.DeleteSeatClassAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
