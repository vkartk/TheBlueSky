namespace TheBlueSky.Bookings.DTOs.Responses.MealPreference
{
    public record MealPreferenceResponse(
        int MealPreferenceId,
        string PreferenceName,
        string? PreferenceDescription,
        bool IsActive
    );

}
