using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.SeatClass;
using TheBlueSky.Flights.Models;

namespace TheBlueSky.Flights.Mappings
{
    public class SeatClassProfile : Profile
    {
        public SeatClassProfile()
        {
            CreateMap<CreateSeatClassRequest, SeatClass>();
            CreateMap<UpdateSeatClassRequest, SeatClass>();
            CreateMap<SeatClass, SeatClassDto>();
        }

    }
}
