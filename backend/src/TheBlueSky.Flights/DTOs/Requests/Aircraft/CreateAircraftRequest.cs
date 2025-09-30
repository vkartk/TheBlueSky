using System.ComponentModel.DataAnnotations;
using TheBlueSky.Flights.Enums;

namespace TheBlueSky.Flights.DTOs.Requests.Aircraft
{
    public record CreateAircraftRequest(

        [Required]
        string OwnerUserId,

        [Required]
        [StringLength(100)]
        string AircraftName,

        [Required]
        [StringLength(100)]
        string AircraftModel,

        [Required]
        AircraftManufacturer Manufacturer,
    
        [Required]
        int EconomySeats,
    
        [Required]
        int BusinessSeats,
    
        [Required]
        int FirstClassSeats
    );
}
