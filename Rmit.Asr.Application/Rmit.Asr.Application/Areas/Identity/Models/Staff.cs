using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.Areas.Identity.Models
{
    public class Staff : ApplicationUser
    {
        public const string EmailSuffix = "rmit.edu.au";
        
        [Display(Name = "Staff ID")]
        public override string Id { get; set; }
    }
}