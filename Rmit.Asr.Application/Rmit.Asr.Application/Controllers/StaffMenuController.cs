using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rmit.Asr.Application.Data;
using Rmit.Asr.Application.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rmit.Asr.Application.Controllers
{
    public class StaffMenuController : Controller
    {
        private readonly ApplicationDataContext _context;

        public StaffMenuController(ApplicationDataContext context)
        {
            _context = context;
        }

        //** STAFF MENU HOME PAGE **//

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        /** CREATE SLOTS **/

        // GET:
        public IActionResult CreateSlot()
        {
            return View();
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSlot([Bind("RoomID,StartTime,StaffID")] Slot slot)
        {
            if (!ModelState.IsValid) return View(slot);

            if (!_context.Room.Any(r => r.RoomID == slot.RoomID))
            {
                ModelState.AddModelError("RoomID", $"Room {slot.RoomID} does not exist.");
            }

            if (!_context.Staff.Any(r => r.StaffID == slot.StaffID))
            {
                ModelState.AddModelError("StaffID", $"Staff {slot.StaffID} does not exist.");
            }

            if (GetAvailableRooms(slot.StartTime).All(r => r.RoomID != slot.RoomID))
            {
                ModelState.AddModelError("RoomID", $"Room {slot.RoomID} has reached a maximum booking of 2 per day.");
            }

            var staffSlotCount = _context.Slot.Count(s => s.StartTime.Date == slot.StartTime.Date && s.StaffID == slot.StaffID);
            if(staffSlotCount >= 4)
            {
                ModelState.AddModelError("StartTime", $"Staff {slot.StaffID} has a maxmimum of 4 bookings at {slot.StartTime:dd-MM-yyyy}.");
            }
            var staffSlotExists = _context.Slot.FirstOrDefault(s => s.RoomID == slot.RoomID && s.StartTime == slot.StartTime && s.StaffID != slot.StaffID);
            if (staffSlotExists != null)
            {
                ModelState.AddModelError("StaffID", $"Slot for staff {staffSlotExists.StaffID} at room {slot.RoomID} {slot.StartTime:dd-MM-yyyy H:mm} already exists.");
            }

            var slotExist = _context.Slot.Any(x => x.RoomID == slot.RoomID && x.StartTime == slot.StartTime);
            if (slotExist)
            {
                ModelState.AddModelError("StaffID", $"Slot already exists.");
            }

            var staffSlot = _context.Slot.FirstOrDefault(x => x.StaffID == slot.StaffID && x.StartTime == slot.StartTime);
            if (staffSlot != null)
            {
                ModelState.AddModelError("StaffID", $"Staff {staffSlot.StaffID} has already been booked at room {staffSlot.RoomID} {staffSlot.StartTime:dd-MM-yyyy H:mm}.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(slot);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(slot);
        }

        /** LIST STAFF **/
       
        // GET:
        public async Task<IActionResult> ListStaff()
        {
            return View(await _context.Staff.ToListAsync());
        }

        /** ROOMS AVAILABLE **/

        //public IActionResult RoomsAvail()
        //{
        //    return View();
        //}

        /// <summary>
        /// Get the available rooms.
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RoomsAvail(DateTime day)
        {

            if (!ModelState.IsValid) return View();
            
            IQueryable<Room> rooms = GetAvailableRooms(day);

            return View(rooms);
        }

        private IQueryable<Room> GetAvailableRooms(DateTime date)
        {
            // Get unavailable rooms
            IQueryable<Room> unavailableRooms = _context.Room
                .Include(r => r.Slots)
                .Where(r => r.Slots.Any(s => s.StartTime.Date == date.Date) && r.Slots.Count >= 2);

            // Compare unavailable rooms and exclude
            IQueryable<Room> rooms = _context.Room
                .Where(r => unavailableRooms.All(x => x.RoomID != r.RoomID));

            return rooms;
        }


        public async Task<IActionResult> RemoveSlot()
        {

            return View();
        }



    }
}
