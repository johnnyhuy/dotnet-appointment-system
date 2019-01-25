using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.Models
{
    public class Student : ApplicationUser
    {
        public const string EmailSuffix = "student.rmit.edu.au";
        
        /// <summary>
        /// Identification string of the student.
        /// </summary>
        [Display(Name = "Student ID")]
        public override string Id { get; set; }
    }
}