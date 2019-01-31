using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Rmit.Asr.Application.Models
{
    public class ApplicationUser : IdentityUser
    {
        /// <inheritdoc />
        [Key]
        [Required]
        [JsonIgnore]
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
        
        [JsonIgnore]
        public override string UserName { get; set; }
        
        [JsonIgnore]
        public override string NormalizedUserName { get; set; }
        
        [JsonIgnore]
        public override string NormalizedEmail { get; set; }
        
        [JsonIgnore]
        public override bool EmailConfirmed { get; set; }
        
        [JsonIgnore]
        public override string PasswordHash { get; set; }
        
        [JsonIgnore]
        public override string SecurityStamp { get; set; }
        
        [JsonIgnore]
        public override string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        
        [JsonIgnore]
        public override string PhoneNumber { get; set; }
        
        [JsonIgnore]
        public override bool PhoneNumberConfirmed { get; set; }
        
        [JsonIgnore]
        public override bool TwoFactorEnabled { get; set; }
        
        [JsonIgnore]
        public override DateTimeOffset? LockoutEnd { get; set; }
        
        [JsonIgnore]
        public override bool LockoutEnabled { get; set; }
        
        [JsonIgnore]
        public override int AccessFailedCount { get; set; }
    }
}