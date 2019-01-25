using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.ValidationAttributes
{
    /// <summary>
    /// Validation attribute copied from the first WDT assignment
    ///
    /// Author: Johnny Huynh <s3604367@student.rmit.edu.au>
    /// </summary>
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