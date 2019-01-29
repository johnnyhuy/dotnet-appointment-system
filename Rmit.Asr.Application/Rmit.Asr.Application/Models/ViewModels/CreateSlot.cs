using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Rmit.Asr.Application.Models.ValidationAttributes;

namespace Rmit.Asr.Application.Models.ViewModels
{
    public class CreateSlot : Slot
    {
        [Required]
        public override string RoomId { get; set; }
        
        [Required]
        [HoursBetween(9, 14)]
        [HourIntervals]
        public override DateTime? StartTime { get; set; }
        
        public IEnumerable<Slot> Slots { get; set; }
        
        public IEnumerable<Room> Rooms { get; set; }
    }
}
