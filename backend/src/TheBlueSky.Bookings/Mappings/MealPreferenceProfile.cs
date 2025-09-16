using AutoMapper;
using TheBlueSky.Bookings.DTOs.Requests.MealPreference;
using TheBlueSky.Bookings.DTOs.Responses.MealPreference;
using TheBlueSky.Bookings.Models;

namespace TheBlueSky.Bookings.Mappings
{
    public class MealPreferenceProfile : Profile
    {
        public MealPreferenceProfile()
        {
            CreateMap<MealPreference, MealPreferenceResponse>();

            CreateMap<CreateMealPreferenceRequest, MealPreference>()
                .ForMember(d => d.MealPreferenceId, opt => opt.Ignore())
                .ForMember(d => d.BookingPassengers, opt => opt.Ignore())
                .ForMember(d => d.IsActive, opt => opt.MapFrom(_ => true));

            CreateMap<UpdateMealPreferenceRequest, MealPreference>()
                .ForMember(d => d.BookingPassengers, opt => opt.Ignore());
        }

    }
}
