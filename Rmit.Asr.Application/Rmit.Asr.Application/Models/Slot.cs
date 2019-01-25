using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.ValidationAttributes;

namespace Rmit.Asr.Application.Models
{
    public class Slot
    {
        [Required]
        public string RoomID { get; set; }
        
        public Room Room { get; set; }

        [Required]
        [HoursBetween(9, 14)]
        [HourIntervals]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [RegularExpression("^e[0-9]{5}$")]
        public string StaffID { get; set; }
        
        public Staff Staff { get; set; }

        [RegularExpression("^s[0 - 9]{7}$")]
        public string StudentID { get; set; }
        
        public Student Student { get; set; }
    }
}
