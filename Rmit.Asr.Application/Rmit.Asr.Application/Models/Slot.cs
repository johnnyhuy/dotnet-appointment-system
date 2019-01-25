using System;
using System.ComponentModel.DataAnnotations;
using Rmit.Asr.Application.ValidationAttributes;

namespace Rmit.Asr.Application.Models
{
    public class Slot
    {
        [Required]
        public string RoomID { get; set; }
        public virtual Room Room { get; set; }

        [HoursBetween(9, 14)]
        [HourIntervals]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Required]
        [RegularExpression("^e[0-9]{5}$")]
        public string StaffID { get; set; }
        public virtual Staff Staff { get; set; }

        [RegularExpression("^s[0 - 9]{7}$")]
        public string StudentID { get; set; }
        public virtual Student Student { get; set; }
    }
}
