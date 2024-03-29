using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.Models
{
    public class Staff : ApplicationUser
    {
        /// <summary>
        /// Constant suffix used for staff emails.
        /// </summary>
        public const string EmailSuffix = "rmit.edu.au";
        
        /// <summary>
        /// Role name of the user.
        /// </summary>
        public const string RoleName = "Staff";

        /// <summary>
        /// Maximum bookings created per day for staff.
        /// </summary>
        public const int MaxBookingPerDay = 4;
        
        /// <summary>
        /// Staff ID name.
        /// </summary>
        [Display(Name = "Staff ID")]
        public virtual string StaffId { get; set; }
    }
}