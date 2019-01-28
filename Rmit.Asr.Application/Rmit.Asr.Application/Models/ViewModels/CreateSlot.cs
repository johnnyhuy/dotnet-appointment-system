using System.Collections.Generic;

namespace Rmit.Asr.Application.Models.ViewModels
{
    public class CreateSlot : Slot
    {
        public IEnumerable<Slot> Slots { get; set; }
        
        public IEnumerable<Room> Rooms { get; set; }
    }
}
