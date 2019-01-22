using Microsoft.AspNetCore.Identity;

namespace Rmit.Asr.Application.Areas.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
    }
}