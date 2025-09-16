namespace TheBlueSky.Bookings.DTOs.Responses.BookingPassenger
{
    public record BookingPassengerResponse(
        int BookingPassengerId,
        int BookingId,
        int PassengerId,
        int FlightSeatStatusId,
        string TicketNumber,
        decimal TicketPrice,
        int? MealPreferenceId
    );

}
