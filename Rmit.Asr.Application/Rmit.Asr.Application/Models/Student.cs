using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.Models
{
    public class Student : ApplicationUser
    {
        /// <summary>
        /// Constant suffix used for student emails.
        /// </summary>
        public const string EmailSuffix = "student.rmit.edu.au";
        
        /// <summary>
        /// Identification string of the student.
        /// </summary>
        [Display(Name = "Student ID")]
        public override string Id { get; set; }
    }
}