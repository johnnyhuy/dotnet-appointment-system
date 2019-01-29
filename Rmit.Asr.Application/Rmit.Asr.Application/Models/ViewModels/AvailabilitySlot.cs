using System;
using System.Collections.Generic;

namespace Rmit.Asr.Application.Models.ViewModels
{
    public class AvailabilitySlot : Slot
    {
        /// <summary>
        /// Select the date to filter available rooms.
        /// </summary>
        public DateTime? Date { get; set; }
        
        /// <summary>
        /// Get all available slots.
        /// </summary>
        public IEnumerable<Slot> AvailableSlots { get; set; }
    }
}
