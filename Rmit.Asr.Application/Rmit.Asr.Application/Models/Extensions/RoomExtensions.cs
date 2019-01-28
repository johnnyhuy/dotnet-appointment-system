using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Rmit.Asr.Application.Models.Extensions
{
    public static class RoomExtensions
    {
        /// <summary>
        /// Get available rooms.
        /// </summary>
        /// <param name="rooms"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static IQueryable<Room> GetAvailableRooms(this IQueryable<Room> rooms, DateTime? date)
        {
            // Get unavailable rooms
            IQueryable<Room> unavailableRooms = rooms
                .Include(r => r.Slots)
                .Where(r => r.Slots.Any(s => s.StartTime != null && s.StartTime.Value.Date == date.Value.Date) && r.Slots.Count >= Room.MaxRoomBookingPerDay);

            // Compare unavailable rooms and exclude
            IQueryable<Room> result = rooms
                .Where(r => unavailableRooms.All(x => x.RoomId != r.RoomId));

            return result;
        }

        /// <summary>
        /// Check if the room is available.
        /// </summary>
        /// <param name="rooms"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public static bool RoomAvailable(this IQueryable<Room> rooms, Slot slot)
        {
            return rooms.GetAvailableRooms(slot.StartTime).Any(r => r.RoomId == slot.RoomId);
        }

        /// <summary>
        /// Check if room exists.
        /// </summary>
        /// <param name="rooms"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public static bool RoomExists(this IQueryable<Room> rooms, string roomId)
        {
            return rooms.Any(s => s.RoomId == roomId);
        }
    }
}