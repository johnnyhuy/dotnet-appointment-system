using System.ComponentModel.DataAnnotations;
using Rmit.Asr.Application.Models.ValidationAttributes;

namespace Rmit.Asr.Application.Models.ViewModels
{
    public class RegisterStaff : Staff
    {
        [StaffId]
        public override string StaffId { get; set; }
        
        [Required]
        public override string FirstName { get; set; }
        
        [Required]
        public override string LastName { get; set; }
    }
}