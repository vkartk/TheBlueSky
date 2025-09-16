using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.Aircraft;
using TheBlueSky.Flights.DTOs.Responses.Aircraft;
using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Mappings
{
    public class AircraftProfile : Profile
    {
        public AircraftProfile()
        {
            CreateMap<Aircraft, AircraftResponse>();
            CreateMap<CreateAircraftRequest, Aircraft>();
            CreateMap<UpdateAircraftRequest, Aircraft>();
        }

    }
}
