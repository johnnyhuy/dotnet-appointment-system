using System.ComponentModel.DataAnnotations;
using Rmit.Asr.Application.Models.ValidationAttributes;

namespace Rmit.Asr.Application.Models.ViewModels
{
    public class RegisterStudent : Student
    {
        [StudentId]
        public override string StudentId { get; set; }
        
        [Required]
        public override string FirstName { get; set; }
        
        [Required]
        public override string LastName { get; set; }
    }
}