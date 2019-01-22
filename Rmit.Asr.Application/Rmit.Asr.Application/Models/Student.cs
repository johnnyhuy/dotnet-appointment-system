using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.Models
{
    public class Student
    {
        [Required]
        public string StudentID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
