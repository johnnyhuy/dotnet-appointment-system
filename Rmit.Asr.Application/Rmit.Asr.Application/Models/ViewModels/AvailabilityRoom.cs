using System;
using System.Collections.Generic;

namespace Rmit.Asr.Application.Models.ViewModels
{
    public class AvailabilityRoom : Room
    {
        /// <summary>
        /// Select the date to filter available rooms.
        /// </summary>
        public DateTime? Date { get; set; }
        
        /// <summary>
        /// Get all available rooms.
        /// </summary>
        public IEnumerable<Room> AvailableRooms { get; set; }
    }
}
