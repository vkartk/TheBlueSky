using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Bookings.DTOs.Requests.Passenger
{
    public class CreatePassengerRequest
    {
        [Required]
        public string ManagedByUserId { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public required DateTime DateOfBirth { get; set; }

        [MaxLength(16)]
        public string? Gender { get; set; }

        [MaxLength(32)]
        public string? PassportNumber { get; set; }

        [MaxLength(2)]
        public string? NationalityCountryId { get; set; }

        [MaxLength(64)]
        public string? RelationshipToManager { get; set; }
    }

}
