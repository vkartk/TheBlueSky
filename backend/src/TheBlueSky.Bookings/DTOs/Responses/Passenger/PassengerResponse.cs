namespace TheBlueSky.Bookings.DTOs.Responses.Passenger
{
    public record PassengerResponse(

        int PassengerId,

        string ManagedByUserId,

        string FirstName,

        string LastName,

        DateTime DateOfBirth,

        string? Gender,

        string? PassportNumber,

        string? NationalityCountryId,

        string? RelationshipToManager,

        DateTime CreatedDate,

        bool IsActive
    );

}
