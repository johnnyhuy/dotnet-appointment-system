using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Rmit.Asr.Application.Models
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Role name of the user.
        /// </summary>
        public virtual string RoleName { get; }
        
        /// <inheritdoc />
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override string Id { get; set; }
        
        /// <summary>
        /// First name of the user.
        /// </summary>
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        /// <summary>
        /// Last name of the user.
        /// </summary>
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}