using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Rmit.Asr.Application.Models
{
    public class Room
    {
        /// <summary>
        /// Maximum slots per room and day.
        /// </summary>
        public const int MaxRoomBookingPerDay = 2;
        
        [Key]
        public string Id { get; set; }
        
        /// <summary>
        /// Room name.
        /// </summary>
        [Display(Name = "Room Name")]
        public virtual string Name { get; set; }

        /// <summary>
        /// Slots related to the room.
        /// </summary>
        [JsonIgnore]
        public ICollection<Slot> Slots { get; set; }
    }
}
