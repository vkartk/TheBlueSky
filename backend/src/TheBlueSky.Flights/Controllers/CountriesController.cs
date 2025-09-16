using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBlueSky.Flights.DTOs.Responses.Country;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Services;

namespace TheBlueSky.Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryResponse>>> GetAllCountries()
        {
            var countries = await _countryService.GetAllCountriesAsync();
            return Ok(countries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryResponse>> GetCountryById(string id)
        {
            var country = await _countryService.GetCountryByIdAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return Ok(country);
        }
    }
}