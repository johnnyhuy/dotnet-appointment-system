using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.ValidationAttributes
{
    public class StaffIdAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var staffId = (string) value;

            if (!staffId.StartsWith('e') || staffId.Length != 6)
            {
                return new ValidationResult(
                    $"The staff ID {staffId} is invalid, it always starts with a letter ‘e’ followed by 5 numbers.");
            }
            
            return ValidationResult.Success;
        }
    }
}