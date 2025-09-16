using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Bookings.DTOs.Requests.Passenger
{
    public record UpdatePassengerRequest(

        [Required]
        int PassengerId,

        [Required]
        int ManagedByUserId,

        [Required]
        [MaxLength(100)]
        string FirstName,

        [Required]
        [MaxLength(100)]
        string LastName,

        [Required]
        [DataType(DataType.Date)]
        DateTime DateOfBirth,

        [MaxLength(16)]
        string? Gender,

        [MaxLength(32)]
        string? PassportNumber,

        [MaxLength(2)]
        string? NationalityCountryId,

        [MaxLength(64)]
        string? RelationshipToManager,

        [Required]
        bool IsActive
    );

}
