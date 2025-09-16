using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Bookings.Models
{
    public class MealPreference
    {
        public int MealPreferenceId { get; set; }

        [MaxLength(64)]
        public string PreferenceName { get; set; } = default!;

        public string? PreferenceDescription { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<BookingPassenger> BookingPassengers { get; set; } = new List<BookingPassenger>();

    }
}
