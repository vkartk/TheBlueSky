using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBlueSky.Bookings.DTOs.Requests.MealPreference;
using TheBlueSky.Bookings.DTOs.Responses.MealPreference;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealPreferenceController : ControllerBase
    {
        private readonly IMealPreferenceService _service;
        public MealPreferenceController(IMealPreferenceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealPreferenceResponse>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MealPreferenceResponse>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);

            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<MealPreferenceResponse>> Create([FromBody] CreateMealPreferenceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _service.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = created.MealPreferenceId }, created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateMealPreferenceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(request);
            if (updated) return NoContent();

            return NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (deleted) return NoContent();

            return NotFound();
        }
    }
}
