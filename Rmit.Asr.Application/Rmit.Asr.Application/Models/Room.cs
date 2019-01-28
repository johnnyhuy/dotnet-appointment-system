using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rmit.Asr.Application.Models
{
    public class Room
    {
        /// <summary>
        /// Maximum slots per room and day.
        /// </summary>
        public const int MaxRoomBookingPerDay = 2;
        
        /// <summary>
        /// Room name.
        /// </summary>
        [Required]
        public string RoomId { get; set; }

        /// <summary>
        /// Slots related to the room.
        /// </summary>
        public ICollection<Slot> Slots { get; set; }
    }
}
