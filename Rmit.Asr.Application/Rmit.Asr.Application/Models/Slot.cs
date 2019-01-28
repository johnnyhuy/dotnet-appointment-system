using System;
using System.ComponentModel.DataAnnotations;
using Rmit.Asr.Application.Models.ValidationAttributes;

namespace Rmit.Asr.Application.Models
{
    public class Slot
    {
        /// <summary>
        /// Room name.
        /// </summary>
        [Required]
        [Display(Name = "Room ID")]
        public string RoomId { get; set; }
        
        /// <summary>
        /// Room related to the slot.
        /// </summary>
        public Room Room { get; set; }

        /// <summary>
        /// Start time date for the slot.
        /// </summary>
        [Required]
        [HoursBetween(9, 14)]
        [HourIntervals]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Time")]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Staff ID who created the slot.
        /// </summary>
        [Display(Name = "Staff ID")]
        public string StaffId { get; set; }
        
        /// <summary>
        /// Staff user related to the slot.
        /// </summary>
        public Staff Staff { get; set; }

        /// <summary>
        /// Student ID who booked the slot.
        /// </summary>
        [Display(Name = "Student ID")]
        public string StudentId { get; set; }
        
        /// <summary>
        /// Student user who booked the slot.
        /// </summary>
        public Student Student { get; set; }
    }
}
