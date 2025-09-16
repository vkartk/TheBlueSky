using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBlueSky.Bookings.DTOs.Requests.BookingPassenger
{
    public record UpdateBookingPassengerRequest(

        [Required]
        int BookingPassengerId,

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
