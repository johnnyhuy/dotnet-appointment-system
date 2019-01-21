using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Rmit.Asr.Application.Models
{
    public class ApplicationUser : IdentityUser
    {
        
        public ICollection<IdentityUserClaim<string>> Claims { get; set; }
        
        public ICollection<IdentityUserLogin<string>> Logins { get; set; }
        
        public ICollection<IdentityUserToken<string>> Tokens { get; set; }
        
        public ICollection<IdentityUserRole<string>> UserRoles { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; }
    }
}