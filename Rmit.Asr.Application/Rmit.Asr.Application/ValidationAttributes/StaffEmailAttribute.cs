using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.ValidationAttributes
{
    public class StaffEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var staffEmail = (string) value;

            if (!staffEmail.EndsWith("@rmit.edu.au"))
            {
                return new ValidationResult(
                    $"The staff email {staffEmail} is invalid, it always ends with rmit.edu.au");
            }
            
            return ValidationResult.Success;
        }
    }
}