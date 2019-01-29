using System;
using System.Collections.Generic;

namespace Rmit.Asr.Application.Models.ViewModels
{
    public class AvailabilitySlot : Slot
    {
        public DateTime? Date { get; set; }
        public IEnumerable<Slot> AvailableSlots { get; set; }
    }
}
