namespace TheBlueSky.Auth.DTOs.Responses
{
    public class LoginResponse
    {
        public string Status { get; set; } = default!;
        public string Message { get; set; } = default!;

        public string? Token { get; set; }

        public DateTime? Expiration { get; set; }

    }
}
