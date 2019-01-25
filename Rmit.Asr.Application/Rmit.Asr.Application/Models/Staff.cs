using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.Models
{
    public class Staff : ApplicationUser
    {
        public const string EmailSuffix = "rmit.edu.au";
        
        /// <summary>
        /// Identification string of the staff.
        /// </summary>
        [Display(Name = "Staff ID")]
        public override string Id { get; set; }
    }
}