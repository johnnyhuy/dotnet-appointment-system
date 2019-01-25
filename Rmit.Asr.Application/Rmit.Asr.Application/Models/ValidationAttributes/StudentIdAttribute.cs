using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.Models.ValidationAttributes
{
    /// <summary>
    /// Validation attribute copied from the first WDT assignment
    ///
    /// Author: Johnny Huynh <s3604367@student.rmit.edu.au>
    /// </summary>
    public class StudentIdAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }
            
            var studentId = (string) value;

            if (!studentId.StartsWith('s') || studentId.Length != 8)
            {
                return new ValidationResult(
                    $"The booked in student ID {studentId} is invalid, it always starts with a letter ‘s’ followed by 7 numbers.");
            }
            
            return ValidationResult.Success;
        }
    }
}