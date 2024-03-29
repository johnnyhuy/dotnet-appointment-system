﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rmit.Asr.Application.Data;
using Rmit.Asr.Application.Models;
using Rmit.Asr.Application.Models.Extensions;
using Rmit.Asr.Application.Models.ViewModels;
using Rmit.Asr.Application.Providers;

namespace Rmit.Asr.Application.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Houses all the staff related functions.
    /// </summary>
    public class SlotController : Controller
    {
        private readonly ApplicationDataContext _context;
        private readonly UserManager<Staff> _staffManager;
        private readonly UserManager<Student> _studentManager;
        private readonly IDateTimeProvider _dateTimeProvider;

        public SlotController(ApplicationDataContext context, UserManager<Staff> staffManager, UserManager<Student> studentManager, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _staffManager = staffManager;
            _studentManager = studentManager;
            _dateTimeProvider = dateTimeProvider;
        }
        
        /// <summary>
        /// Slot view for the index of slots.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            IOrderedQueryable<Slot> slots = _context.Slot
                .Include(s => s.Room)
                .Include(s => s.Staff)
                .Include(s => s.Student)
                .OrderBy(s => s.StartTime);
            
            return View(slots);
        }
        
        /// <summary>
        /// List of the logged in staff slots.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = Staff.RoleName)]
        public async Task<IActionResult> StaffIndex()
        {
            Staff staff = await _staffManager.GetUserAsync(User);
            
            IOrderedQueryable<Slot> slots = _context.Slot
                .Include(s => s.Room)
                .Include(s => s.Staff)
                .Include(s => s.Student)
                .Where(s => s.StaffId == staff.Id)
                .OrderBy(s => s.StartTime);
            
            return View(slots);
        }

        /// <summary>
        /// Create slot form.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = Staff.RoleName)]
        public IActionResult Create()
        {
            IOrderedQueryable<Slot> slots = _context.Slot
                .Include(s => s.Room)
                .Include(s => s.Staff)
                .Include(s => s.Student)
                .OrderBy(s => s.StartTime);

            var slot = new CreateSlot
            {
                Slots = slots,
                Rooms = _context.Room.OrderBy(r => r.Name)
            };
            
            return View(slot);
        }

        /// <summary>
        /// POST request to create a slot.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Staff.RoleName)]
        public async Task<IActionResult> Create([Bind("RoomId,StartTime")] CreateSlot slot)
        {
            IOrderedQueryable<Slot> slots = _context.Slot
                .Include(s => s.Room)
                .Include(s => s.Staff)
                .Include(s => s.Student)
                .OrderBy(s => s.StartTime);
            
            // Add logged in user to slot
            Staff staff = await _staffManager.GetUserAsync(User);
            slot.StaffId = staff.Id;

            // Load navigation properties
            slot.Slots = slots;
            slot.Rooms = _context.Room;
            
            if (!ModelState.IsValid) return View(slot);

            if (!_context.Room.RoomExists(slot.RoomId))
            {
                ModelState.AddModelError("RoomName", "Room does not exist.");
            }
            else if (!_context.Room.RoomAvailable(slot))
            {
                ModelState.AddModelError("RoomName", $"Room has reached a maximum booking of {Room.MaxRoomBookingPerDay} per day.");
            }

            if (slot.StartTime < _dateTimeProvider.Now())
            {
                ModelState.AddModelError("StartTime", "Slot cannot be created in the past.");
            }

            if (_context.Slot.GetStaffDailySlotCount(slot) >= Staff.MaxBookingPerDay)
            {
                ModelState.AddModelError("StartTime", $"Staff has a maximum of {Staff.MaxBookingPerDay} bookings.");
            }

            Slot alreadyTakenSlot = _context.Slot.GetAlreadyTakenSlot(slot).Include(s => s.Staff).FirstOrDefault();
            if (alreadyTakenSlot != null)
            {
                ModelState.AddModelError("RoomName", "Staff has already taken slot.");
            }

            if (_context.Slot.SlotExists(slot))
            {
                ModelState.AddModelError("RoomName", "Slot already exists.");
            }

            Slot staffAlreadyCreatedSlot = _context.Slot.GetStaffSlot(slot).FirstOrDefault();
            if (staffAlreadyCreatedSlot != null)
            {
                ModelState.AddModelError("RoomName", "You have already created a slot on the same time with a different room.");
            }

            if (!ModelState.IsValid) return View(slot);

            _context.Slot.Add(slot);
            await _context.SaveChangesAsync();
            
            TempData["StatusMessage"] = "Successfully created slot!";
            TempData["AlertType"] = "success";

            return RedirectToAction("Index");
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
            // TODO: Move error page to index flash message.
            if (!ModelState.IsValid) return View(slot);

            if (_context.Slot.SlotBookedByStudent(slot))
                ModelState.AddModelError("StartTime", "Cannot remove slot as a student has been booked into it.");
            
            Slot deleteSlot = _context.Slot.Include(s => s.Room).GetSlot(slot).FirstOrDefault();
            if (deleteSlot == null)
            {
                ModelState.AddModelError(string.Empty, "Slot does not exist.");
            }
            
            if (!ModelState.IsValid || deleteSlot == null) return View(slot);

            _context.Slot.Remove(deleteSlot);

            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = "Successfully removed slot!";
            TempData["AlertType"] = "success";

            return RedirectToAction("Index");
        }
        
        /// <summary>
        /// Form to make a booking.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Student.RoleName)]
        public IActionResult Book()
        {
            IOrderedQueryable<Slot> slots = _context.Slot
                .Include(s => s.Room)
                .Include(s => s.Staff)
                .Include(s => s.Student)
                .OrderBy(s => s.StartTime);

            var slot = new BookSlot
            {
                Slots = slots,
                Rooms = _context.Room.OrderBy(r => r.Name)
            };
            
            return View(slot);
        }

        /// <summary>
        /// Post request to book a slot.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Student.RoleName)]
        public async Task<IActionResult> Book([Bind("RoomId,StartTime")] BookSlot slot)
        {
            slot.Slots = _context.Slot
                .Include(s => s.Room)
                .Include(s => s.Staff)
                .Include(s => s.Student)
                .OrderBy(s => s.StartTime);
            slot.Rooms = _context.Room.OrderBy(r => r.Name);
            
            Student student = await _studentManager.GetUserAsync(User);
            slot.StudentId = student.Id;
            
            if (!ModelState.IsValid) return View(slot);
            
            Slot updateSlot = _context.Slot.Include(s => s.Room).GetSlot(slot).FirstOrDefault();

            if (updateSlot == null)
            {
                ModelState.AddModelError("StartTime", "Slot does not exist.");
            }

            if (slot.StartTime < _dateTimeProvider.Now())
            {
                ModelState.AddModelError("StartTime", "Slot cannot be booked in the past.");
            }

            if (_context.Slot.Any(s => s.StartTime.Value.Date == slot.StartTime.Value.Date && s.StudentId == slot.StudentId))
            {
                ModelState.AddModelError("StartTime", "Student has reached their maximum bookings for this day.");
            }

            if (!_context.Room.RoomExists(slot.RoomId))
            {
                ModelState.AddModelError("RoomName", "Room does not exist.");
            }

            Slot studentBookedSlot = _context.Slot.Include(s => s.Student).GetSlot(slot).FirstOrDefault(s => s.StudentId != student.Id && !string.IsNullOrEmpty(s.StudentId));
            if (studentBookedSlot != null)
            {
                ModelState.AddModelError("StartTime", "Student has already booked slot in room.");
            }

            if (!ModelState.IsValid || updateSlot == null) return View(slot);
            
            updateSlot.StudentId = slot.StudentId;
            
            _context.Slot.Update(updateSlot);

            await _context.SaveChangesAsync();
            
            TempData["StatusMessage"] = "Successfully booked slot!";
            TempData["AlertType"] = "success";

            return RedirectToAction("Index", "Slot");
        }
    
        /// <summary>
        /// Form to cancel a slot.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Student.RoleName)]
        public IActionResult Cancel()
        {
            IOrderedQueryable<Slot> slots = _context.Slot
                .Include(s => s.Room)
                .Include(s => s.Staff)
                .Include(s => s.Student)
                .OrderBy(s => s.StartTime);

            var slot = new CancelSlot
            {
                Slots = slots,
                Rooms = _context.Room.OrderBy(r => r.Name)
            };
            
            return View(slot);
        }

        /// <summary>
        /// POST request to cancel a slot.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Student.RoleName)]
        public async Task<IActionResult> Cancel([Bind("RoomId,StartTime")] CancelSlot slot)
        {
            slot.Slots = _context.Slot
                .Include(s => s.Room)
                .Include(s => s.Staff)
                .Include(s => s.Student)
                .OrderBy(s => s.StartTime);
            slot.Rooms = _context.Room.OrderBy(r => r.Name);
            
            Student student = await _studentManager.GetUserAsync(User);
            slot.StudentId = student.Id;
            
            if (!ModelState.IsValid) return View(slot);
            
            Slot updateSlot = _context.Slot.Include(s => s.Room).GetSlot(slot).FirstOrDefault();

            if (updateSlot == null)
            {
                ModelState.AddModelError("StartTime", "Slot does not exist.");
            }

            if (!_context.Room.RoomExists(slot.RoomId))
            {
                ModelState.AddModelError("RoomName", "Room does not exist.");
            }

            if (_context.Slot.GetSlot(slot).Any(s => s.StudentId == null))
            {
                ModelState.AddModelError("StartTime", "No student is booked in room.");
            }

            Slot otherStudentBookedSlot = _context.Slot.Include(s => s.Student).GetSlot(slot).FirstOrDefault(s => s.StudentId != slot.StudentId && !string.IsNullOrEmpty(s.StudentId));
            if (otherStudentBookedSlot != null)
            {
                ModelState.AddModelError("StartTime", "Other student booked slot.");
            }

            if (!ModelState.IsValid || updateSlot == null) return View(slot);
            
            updateSlot.StudentId = null;
            
            _context.Slot.Update(updateSlot);

            await _context.SaveChangesAsync();
            
            TempData["StatusMessage"] = "Successfully cancelled slot!";
            TempData["AlertType"] = "success";

            return RedirectToAction("Index", "Slot");
        }
        
        /// <summary>
        /// Get available slots.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Student.RoleName)]
        public IActionResult AvailabilityIndex()
        {
            var slot = new AvailabilitySlot
            {
                Date = _dateTimeProvider.Now().Date,
                AvailableSlots = _context.Slot
                    .Include(s => s.Room)
                    .Include(s => s.Staff)
                    .Where(s => s.StartTime.Value.Date == _dateTimeProvider.Now().Date && string.IsNullOrEmpty(s.StudentId))
            };

            return View(slot);
        }
        
        /// <summary>
        /// Get available slots by date.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("AvailabilityByDateIndex")]
        [Authorize(Roles = Student.RoleName)]
        public IActionResult AvailabilityIndex([Bind("Date")]AvailabilitySlot slot)
        {
            if (!ModelState.IsValid) return View(slot);

            slot.AvailableSlots = _context.Slot
                .Include(s => s.Room)
                .Include(s => s.Staff)
                .Where(s => s.StartTime > _dateTimeProvider.Now())
                .Where(s => s.StartTime.Value.Date == slot.Date && string.IsNullOrEmpty(s.StudentId));

            return View(slot);
        }
    }
}
