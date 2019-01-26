using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.Models
{
    public class Staff : ApplicationUser
    {
        /// <summary>
        /// Constant suffix used for staff emails.
        /// </summary>
        public const string EmailSuffix = "rmit.edu.au";
        
        /// <inheritdoc />
        public override string RoleName => "Staff";
        
        /// <inheritdoc />
        [Display(Name = "Staff ID")]
        public override string Id { get; set; }
    }
}