using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Rmit.Asr.Application.Areas.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; }
    }
}