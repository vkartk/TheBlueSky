using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Flights.DTOs.Requests.SeatClass
{
    public record CreateSeatClassRequest(
    
        [Required]
        [StringLength(50)]
        string ClassName,

        [StringLength(255)]
        string? ClassDescription,

        [Required]
        int PriorityOrder
    );

}
