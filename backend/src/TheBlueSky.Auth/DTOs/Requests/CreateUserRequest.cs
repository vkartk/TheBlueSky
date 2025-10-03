using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Auth.DTOs.Requests
{
    public class CreateUserRequest
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = default!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = default!;


        [Required,EmailAddress]
        public string Email { get; set; } = default!;


        [Required, MinLength(8)]
        public string Password { get; set; } = default!;
    }
}
