using AutoMapper;
using TheBlueSky.Flights.DTOs.Responses.Country;
using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Mappings
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<Country, CountryResponse>();
        }
    }
}
