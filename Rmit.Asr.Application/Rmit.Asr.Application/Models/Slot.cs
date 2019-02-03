using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Rmit.Asr.Application.Models
{
    public class Slot
    {
        /// <summary>
        /// Room name.
        /// </summary>
        [JsonIgnore]
        [Display(Name = "Room ID")]
        public virtual string RoomId { get; set; }
        
        /// <summary>
        /// Room related to the slot.
        /// </summary>
        public Room Room { get; set; }

        /// <summary>
        /// Start time date for the slot.
        /// </summary>
        [Display(Name = "Start Time")]
        public virtual DateTime? StartTime { get; set; }

        /// <summary>
        /// Staff ID who created the slot.
        /// </summary>
        [JsonIgnore]
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
