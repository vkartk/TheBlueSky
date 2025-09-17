using System.ComponentModel.DataAnnotations;

namespace TheBlueSky.Bookings.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CurrencyAmountAttribute : ValidationAttribute
    {
        public int Scale { get; }

        public CurrencyAmountAttribute(int scale = 2)
        {
            if (scale < 0 || scale > 6)
                throw new ArgumentOutOfRangeException(nameof(scale), "Scale must be between 0 and 6.");
            Scale = scale;
            ErrorMessage = $"Amount must be >= 0 with at most {scale} decimal place(s).";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext _)
        {
            if (value is null) return ValidationResult.Success;

            if (value is not decimal dec)
                return new ValidationResult("Invalid currency amount.");

            if (dec < 0) return new ValidationResult(ErrorMessage);

            // Reject more fractional digits than Scale (e.g., 12.345 when Scale=2)
            var rounded = decimal.Round(dec, Scale, MidpointRounding.AwayFromZero);
            if (dec != rounded) return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }

    }
}
