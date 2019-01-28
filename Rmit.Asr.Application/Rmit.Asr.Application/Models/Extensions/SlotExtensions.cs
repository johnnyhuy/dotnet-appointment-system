using System.Linq;

namespace Rmit.Asr.Application.Models.Extensions
{
    public static class SlotExtensions
    {
        /// <summary>
        /// Get slot based on room ID and start time.
        /// Which are composite keys in the database.
        /// </summary>
        /// <param name="slots"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public static IQueryable<Slot> GetSlot(this IQueryable<Slot> slots, Slot slot)
        {
            return slots.Where(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime);
        }

        /// <summary>
        /// Get already taken slot.
        /// </summary>
        /// <param name="slots"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public static IQueryable<Slot> GetAlreadyTakenSlot(this IQueryable<Slot> slots, Slot slot)
        {
            return slots.GetSlot(slot).Where(s => s.StaffId != slot.StaffId);
        }
        
        /// <summary>
        /// Get already taken slot.
        /// </summary>
        /// <param name="slots"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public static IQueryable<Slot> GetStaffSlot(this IQueryable<Slot> slots, Slot slot)
        {
            return slots.Where(s => s.StaffId == slot.StaffId && s.StartTime == slot.StartTime);
        }
        
        /// <summary>
        /// Get the staff daily slot count.
        /// </summary>
        /// <param name="slots"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public static int GetStaffDailySlotCount(this IQueryable<Slot> slots, Slot slot)
        {
            return slots
                .Count(s => s.StartTime != null && 
                            s.StartTime.Value.Date == slot.StartTime.Value.Date &&
                            s.StaffId == slot.StaffId);
        }
        
        /// <summary>
        /// Check if slot exists.
        /// </summary>
        /// <param name="slots"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public static bool SlotExists(this IQueryable<Slot> slots, Slot slot)
        {
            return slots.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime);
        }
    }
}