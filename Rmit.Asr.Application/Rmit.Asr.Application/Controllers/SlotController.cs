using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Data;
using Rmit.Asr.Application.Models;
using Rmit.Asr.Application.Models.Extensions;
using Rmit.Asr.Application.Models.ViewModels;

namespace Rmit.Asr.Application.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Houses all the staff related functions.
    /// </summary>
    public class SlotController : Controller
    {
        private readonly ApplicationDataContext _context;
        private readonly UserManager<Staff> _userManager;

        public SlotController(ApplicationDataContext context, UserManager<Staff> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Student view for the index of slots.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = Student.RoleName)]
        public IActionResult StudentIndex()
        {
            return View(_context.Slot);
        }
        
        /// <summary>
        /// Staff view for the index of slots.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = Staff.RoleName)]
        public IActionResult StaffIndex()
        {
            return View(_context.Slot);
        }

        /// <summary>
        /// Create slot form.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = Staff.RoleName)]
        public IActionResult Create()
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
        [Authorize(Roles = Staff.RoleName)]
        public async Task<IActionResult> Create([Bind("RoomId,StartTime")] Slot slot)
        {
            if (!ModelState.IsValid) return View(slot);

            if (!_context.Room.RoomExists(slot.RoomId))
            {
                ModelState.AddModelError("RoomID", $"Room {slot.RoomId} does not exist.");
            }
            else if (!_context.Room.RoomAvailable(slot))
            {
                ModelState.AddModelError("RoomID", $"Room {slot.RoomId} has reached a maximum booking of {Room.MaxRoomBookingPerDay} per day.");
            }

            if (_context.Slot.GetStaffDailySlotCount(slot) >= Staff.MaxBookingPerDay)
            {
                ModelState.AddModelError("StartTime", $"Staff {slot.StaffId} has a maximum of {Staff.MaxBookingPerDay} bookings at {slot.StartTime:dd-MM-yyyy}.");
            }

            Slot alreadyTakenSlot = _context.Slot.GetAlreadyTakenSlot(slot).FirstOrDefault();
            if (alreadyTakenSlot != null)
            {
                ModelState.AddModelError("StaffID", $"Staff {alreadyTakenSlot.StaffId} has already taken slot at room {slot.RoomId} {slot.StartTime:dd-MM-yyyy H:mm}.");
            }

            if (_context.Slot.SlotExists(slot))
            {
                ModelState.AddModelError("StaffID", $"Slot at room {slot.RoomId} {slot.StartTime:dd-MM-yyyy H:mm} already exists.");
            }

            Slot staffAlreadyCreatedSlot = _context.Slot.GetStaffSlot(slot).FirstOrDefault();
            if (staffAlreadyCreatedSlot != null)
            {
                ModelState.AddModelError("StaffID", $"You have already created a slot at room {staffAlreadyCreatedSlot.RoomId} {staffAlreadyCreatedSlot.StartTime:dd-MM-yyyy H:mm}.");
            }

            if (!ModelState.IsValid) return View(slot);
            
            Staff staff = await _userManager.GetUserAsync(User);
            slot.StaffId = staff.Id;
            
            _context.Slot.Add(slot);
            await _context.SaveChangesAsync();

            return RedirectToAction("StaffIndex");
        }

        /// <summary>
        /// POST request to remove a slot.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Staff.RoleName)]
        public async Task<IActionResult> Remove([Bind("RoomId,StartTime")] RemoveSlot slot)
        {
            if (!ModelState.IsValid) return View(slot);

            if (_context.Slot.SlotBookedByStudent(slot))
                ModelState.AddModelError("StudentId", "Cannot remove slot as a student has been booked into it.");
            
            Slot deleteSlot = _context.Slot.GetSlot(slot).FirstOrDefault();
            if (deleteSlot == null)
            {
                ModelState.AddModelError(string.Empty, $"Slot at room {slot.RoomId} {slot.StartTime:dd-MM-yyyy H:mm} does not exist.");
            }
            
            if (!ModelState.IsValid || deleteSlot == null) return View(slot);

            _context.Slot.Remove(deleteSlot);

            await _context.SaveChangesAsync();

            return RedirectToAction("StaffIndex");
        }
    }
}
