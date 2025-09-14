using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Auth.DTOs.Requests
{
    public class CreateUserRequest
    {
        [Required,EmailAddress]
        public string Email { get; set; } = default!;


        [Required, MinLength(8)]
        public string Password { get; set; } = default!;
    }
}
