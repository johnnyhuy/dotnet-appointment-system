using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.ValidationAttributes
{
    /// <summary>
    /// Validation attribute copied from the first WDT assignment
    ///
    /// Author: Johnny Huynh <s3604367@student.rmit.edu.au>
    /// </summary>
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