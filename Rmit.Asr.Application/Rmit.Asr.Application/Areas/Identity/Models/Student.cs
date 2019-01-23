using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.Areas.Identity.Models
{
    public class Student : ApplicationUser
    {
        public const string EmailSuffix = "student.rmit.edu.au";
        
        [Display(Name = "Student ID")]
        public override string Id { get; set; }
    }
}