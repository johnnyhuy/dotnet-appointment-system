using System;
using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.Models
{
    public class Staff
    {
        [Required]
        public string StaffID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

    }
}
