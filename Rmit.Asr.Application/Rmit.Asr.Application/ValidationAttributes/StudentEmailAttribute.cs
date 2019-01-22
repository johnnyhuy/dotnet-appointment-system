using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.ValidationAttributes
{
    public class StudentEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var studentEmail = (string) value;

            if (!studentEmail.EndsWith("@student.rmit.edu.au"))
            {
                return new ValidationResult(
                    $"The student email {studentEmail} is invalid, it always ends with student.rmit.edu.au");
            }
            
            return ValidationResult.Success;
        }
    }
}