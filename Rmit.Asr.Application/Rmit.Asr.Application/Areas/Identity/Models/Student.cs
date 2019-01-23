using System.ComponentModel.DataAnnotations;
using Rmit.Asr.Application.ValidationAttributes;

namespace Rmit.Asr.Application.Areas.Identity.Models
{
    public class Student : ApplicationUser
    {
        public const string EmailSuffix = "student.rmit.edu.au";
        
        [StudentId]
        [Display(Name = "Student ID")]
        public override string Id { get; set; }

        public override string Email => $"{Id}@{EmailSuffix}";
    }
}