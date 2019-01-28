using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rmit.Asr.Application.Data;
using Rmit.Asr.Application.Models;
using Rmit.Asr.Application.Models.Extensions;
using Rmit.Asr.Application.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rmit.Asr.Application.Controllers
{
    [Authorize(Roles = Staff.RoleName)]
    public class StaffMenuController : Controller
    {
        private readonly ApplicationDataContext _context;
        private readonly UserManager<Staff> _userManager;

        public StaffMenuController(ApplicationDataContext context, UserManager<Staff> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        /// <summary>
        /// POST request to create a slot.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSlot([Bind("RoomId,StartTime")] Slot slot)
        {
            if (!ModelState.IsValid) return View(slot);

            if (!_context.Room.Any(r => r.RoomId == slot.RoomId))
            {
                ModelState.AddModelError("RoomID", $"Room {slot.RoomId} does not exist.");
            }
            else if (_context.Room.GetAvailableRooms(slot.StartTime).All(r => r.RoomId != slot.RoomId))
            {
                ModelState.AddModelError("RoomID", $"Room {slot.RoomId} has reached a maximum booking of {Room.MaxRoomBookingPerDay} per day.");
            }

            int staffDailySlotCount = _context.Slot.Count(s => s.StartTime != null && s.StartTime.Value.Date == slot.StartTime.Value.Date && s.StaffId == slot.StaffId);
            if (staffDailySlotCount >= 4)
            {
                ModelState.AddModelError("StartTime", $"Staff {slot.StaffId} has a maximum of {Staff.MaxBookingPerDay} bookings at {slot.StartTime:dd-MM-yyyy}.");
            }
            
            Slot staffAlreadyTakenSlot = _context.Slot.FirstOrDefault(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StaffId != slot.StaffId);
            if (staffAlreadyTakenSlot != null)
            {
                ModelState.AddModelError("StaffID", $"Staff {staffAlreadyTakenSlot.StaffId} has already taken slot at room {slot.RoomId} {slot.StartTime:dd-MM-yyyy H:mm}.");
            }

            bool slotExists = _context.Slot.Any(x => x.RoomId == slot.RoomId && x.StartTime == slot.StartTime);
            if (slotExists)
            {
                ModelState.AddModelError("StaffID", $"Slot at room {slot.RoomId} {slot.StartTime:dd-MM-yyyy H:mm} already exists.");
            }

            Slot staffAlreadyCreatedSlot = _context.Slot.FirstOrDefault(x => x.StaffId == slot.StaffId && x.StartTime == slot.StartTime);
            if (staffAlreadyCreatedSlot != null)
            {
                ModelState.AddModelError("StaffID", $"You have already created a slot at room {staffAlreadyCreatedSlot.RoomId} {staffAlreadyCreatedSlot.StartTime:dd-MM-yyyy H:mm}.");
            }

            if (!ModelState.IsValid) return View(slot);
            
            Staff staff = await _userManager.GetUserAsync(User);
            slot.StaffId = staff.Id;
            
            _context.Slot.Add(slot);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        /** LIST STAFF **/
       
        // GET:
        public async Task<IActionResult> ListStaff()
        {
            return View(await _context.Staff.ToListAsync());
        }

        /** ROOMS AVAILABLE **/

        [HttpGet]
        public IActionResult RoomsAvail(DateTime day)
        {

            if (!ModelState.IsValid) return View();
            
            IQueryable<Room> rooms = _context.Room.GetAvailableRooms(day);

            return View(rooms);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveSlot([Bind("RoomID,StartTime")] RemoveSlot slot)
        {
            if (!ModelState.IsValid) return View(slot);

            var studentBookedIn = _context.Slot.Any(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime && s.StudentId != null);
            if (studentBookedIn)
            {
                ModelState.AddModelError("StudentID", "Cannot remove slot as student has been booked into it.");

            }

            if (ModelState.IsValid)
            {
                var deleteSlot = _context.Slot.Where(s => s.RoomId == slot.RoomId && s.StartTime == slot.StartTime).FirstOrDefault();
                _context.Slot.Remove(deleteSlot);

                await _context.SaveChangesAsync();

                return RedirectToAction("ShowSlots");
            }

            return View(slot);

        }

        public IActionResult ShowSlots()
        {
            return View(_context.Slot);
        }
    }
}
