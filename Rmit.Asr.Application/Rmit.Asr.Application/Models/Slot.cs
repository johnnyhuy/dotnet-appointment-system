using System;
using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.Models
{
    public class Slot
    {
        [Required]
        public string RoomID { get; set; }
        public virtual Room Room { get; set; }

        public DateTime StartTime { get; set; }

        [Required]
        public string StaffID { get; set; }
        public virtual Staff Staff { get; set; }

        public string StudentID { get; set; }
        public virtual Student Student { get; set; }
    }
}
