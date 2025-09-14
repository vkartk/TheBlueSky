using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Auth.DTOs.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;
    }
}
