using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Bookings.DTOs.Requests.MealPreference
{
    public class UpdateMealPreferenceRequest
    {
        [Required]
        public required int MealPreferenceId { get; set; }

        [Required]
        [MaxLength(64)]
        public string PreferenceName { get; set; } = string.Empty;

        public string? PreferenceDescription { get; set; }

        [Required]
        public required bool IsActive { get; set; }
    }

}
