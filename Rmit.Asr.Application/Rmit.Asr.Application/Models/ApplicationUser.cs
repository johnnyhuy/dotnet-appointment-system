using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Rmit.Asr.Application.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}