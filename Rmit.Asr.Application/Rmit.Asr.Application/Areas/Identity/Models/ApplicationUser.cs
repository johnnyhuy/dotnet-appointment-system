using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Rmit.Asr.Application.Areas.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Key]
        [Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
    }
}