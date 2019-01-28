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
        /// Role name of the user.
        /// </summary>
        public const string RoleName = "Student";
        
        /// <summary>
        /// Staff ID name.
        /// </summary>
        [Display(Name = "Student ID")]
        public virtual string StudentId { get; set; }
    }
}