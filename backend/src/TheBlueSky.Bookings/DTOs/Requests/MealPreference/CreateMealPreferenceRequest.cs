using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Bookings.DTOs.Requests.MealPreference
{
    public record CreateMealPreferenceRequest(

        [Required]
        [MaxLength(64)]
        string PreferenceName,

        string? PreferenceDescription
    );
}