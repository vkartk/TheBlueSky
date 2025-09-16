using AutoMapper;
using TheBlueSky.Flights.DTOs.Responses.Country;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories;

namespace TheBlueSky.Flights.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryService(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CountryResponse>> GetAllCountriesAsync()
        {
            var countries = await _countryRepository.GetAllCountriesAsync();
            return _mapper.Map<IEnumerable<CountryResponse>>(countries);
        }

        public async Task<CountryResponse?> GetCountryByIdAsync(string id)
        {
            var country = await _countryRepository.GetCountryByIdAsync(id);
            if (country == null)
            {
                return null;
            }
            return _mapper.Map<CountryResponse>(country);
        }
    }
}
