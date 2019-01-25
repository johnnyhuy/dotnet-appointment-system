using System;
using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.ValidationAttributes
{
    /// <summary>
    /// Validation attribute copied from the first WDT assignment
    ///
    /// Author: Johnny Huynh <s3604367@student.rmit.edu.au>
    /// </summary>
    public class HoursBetweenAttribute : ValidationAttribute
    {
        private readonly int _startHour;
        private readonly int _endHour;

        public HoursBetweenAttribute(int startHour, int endHour)
        {
            _startHour = startHour;
            _endHour = endHour;
        }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var time = (DateTime) value;
            
            if (time.Hour < _startHour || time.Hour > _endHour)
                return new ValidationResult($"The date time field {time} must be between {_startHour}:00 - {_endHour}:00");
            
            return ValidationResult.Success;
        }
    }
}