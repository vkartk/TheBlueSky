﻿using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Auth.DTOs.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;
    }
}
