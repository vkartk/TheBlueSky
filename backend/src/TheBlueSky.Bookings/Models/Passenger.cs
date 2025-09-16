using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Bookings.Models
{
    public class Passenger
    {
        public int PassengerId { get; set; }

        public int ManagedByUserId { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; } = default!;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = default!;

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(16)]
        public string? Gender { get; set; }

        [MaxLength(32)]
        public string? PassportNumber { get; set; }

        [MaxLength(2)]
        public string? NationalityCountryId { get; set; }

        [MaxLength(64)]
        public string? RelationshipToManager { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        public ICollection<BookingPassenger> Bookings { get; set; } = new List<BookingPassenger>();

    }
}
