using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rmit.Asr.Application.Areas.Identity.Models
{
    public class Student : ApplicationUser
    {
        private const string EmailSuffix = "student.rmit.edu.au";
        
        [Key]
        [Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public override string Email => $"{Id}@{EmailSuffix}";
    }
}