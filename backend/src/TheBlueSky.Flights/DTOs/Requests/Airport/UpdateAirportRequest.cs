using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Flights.DTOs.Requests.Airport
{
    public record UpdateAirportRequest(

        [Required] 
        int AirportId,

        [Required]
        [StringLength(3)] 
        string AirportCode,

        [Required]
        [StringLength(200)] 
        string AirportName,

        [Required]
        [StringLength(100)] 
        string City,

        [Required] 
        string CountryId,

        bool IsActive
    );

}
