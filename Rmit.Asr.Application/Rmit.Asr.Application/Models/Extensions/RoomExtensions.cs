using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Rmit.Asr.Application.Models.Extensions
{
    public static class RoomExtensions
    {
        public static IQueryable<Room> GetAvailableRooms(this IQueryable<Room> rooms, DateTime? date)
        {
            // Get unavailable rooms
            IQueryable<Room> unavailableRooms = rooms
                .Include(r => r.Slots)
                .Where(r => r.Slots.Any(s => s.StartTime != null && s.StartTime.Value.Date == date.Value.Date) && r.Slots.Count >= 2);

            // Compare unavailable rooms and exclude
            IQueryable<Room> result = rooms
                .Where(r => unavailableRooms.All(x => x.RoomId != r.RoomId));

            return result;
        }
    }
}