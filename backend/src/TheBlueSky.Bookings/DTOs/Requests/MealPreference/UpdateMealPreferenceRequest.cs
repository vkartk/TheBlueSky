using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Bookings.DTOs.Requests.MealPreference
{
    public record UpdateMealPreferenceRequest(

        [Required]
        int MealPreferenceId,

        [Required]
        [MaxLength(64)]
        string PreferenceName,

        string? PreferenceDescription,

        [Required]
        bool IsActive
    );

}
