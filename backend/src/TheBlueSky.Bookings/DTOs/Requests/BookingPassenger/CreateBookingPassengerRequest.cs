using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Bookings.DTOs.Requests.BookingPassenger
{
    public record CreateBookingPassengerRequest(

        [Required]
        int BookingId,

        [Required]
        int PassengerId,

        [Required]
        int FlightSeatStatusId,

        [Required]
        [MaxLength(32)]
        string TicketNumber,

        [Range(0, double.MaxValue)]
        decimal TicketPrice,

        int? MealPreferenceId
    );

}
