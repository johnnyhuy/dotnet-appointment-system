using System.ComponentModel.DataAnnotations;
using Rmit.Asr.Application.ValidationAttributes;

namespace Rmit.Asr.Application.Areas.Identity.Models
{
    public class Staff : ApplicationUser
    {
        public const string EmailSuffix = "rmit.edu.au";
        
        [StaffId]
        [Display(Name = "Staff ID")]
        public override string Id { get; set; }

        public override string Email => $"{Id}@{EmailSuffix}";
    }
}