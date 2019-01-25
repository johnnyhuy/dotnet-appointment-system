using System;
using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.Models.ValidationAttributes
{
    /// <summary>
    /// Validation attribute copied from the first WDT assignment
    ///
    /// Author: Johnny Huynh <s3604367@student.rmit.edu.au>
    /// </summary>
    public class HourIntervalsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startTime = (DateTime) value;
            
            if (startTime.Minute != 0 || startTime.Second != 0 || startTime.Millisecond != 0)
                return new ValidationResult($"Slot start time {startTime} must be in 1 hour intervals e.g. 9:00, 13:00, 22:00");
            
            return ValidationResult.Success;
        }
    }
}