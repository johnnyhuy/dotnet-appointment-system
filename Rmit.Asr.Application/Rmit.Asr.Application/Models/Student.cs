using System;
using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.Models
{
    public class Student
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; }
    }
}