using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Data;
using Rmit.Asr.Application.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rmit.Asr.Application.Controllers
{
    public class StudentMenuController : Controller
    {
        private readonly ApplicationDataContext _context;

        public StudentMenuController(ApplicationDataContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // GET:
        public IActionResult ListStudents()
        {
            return View(_context.Student.ToList() );
        }


        [HttpGet]
        public IActionResult StaffAvail(DateTime day)
        {

            if (!ModelState.IsValid) return View();

            // gets all slots for that day that dont have a student booked into it
            // we know that staff cannot create a new slot if they have reached thier max bookings for that day
            // so all these slots must mean that these staff members are available
            var availStaff = _context.Slot.Where(x => x.StartTime.Value.Date == day.Date && x.StudentId == null);


            return View(availStaff);



        }


        // GET:
        public IActionResult MakeBooking()
        {
            return View();
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeBooking([Bind("RoomId,StartTime,StudentId")] Slot slot)
        {
            if (!ModelState.IsValid) return View(slot);

            // A student can only mkae one booking per day
            var amountOfBookings = _context.Slot.Count(x => x.StartTime.Value.Date == slot.StartTime.Value.Date && x.StudentId == slot.StudentId);
            if(amountOfBookings != 0) // > 0 or >= 1
            {
                ModelState.AddModelError("StudentId", $"Student {slot.StudentId} has reached their maximum bookings for this day ({slot.StartTime.Value.Date:dd-MM-yyyy})");
            }


            if (!_context.Room.Any(r => r.RoomId == slot.RoomId))
            {
                ModelState.AddModelError("RoomId", $"Room {slot.RoomId} does not exist.");
            }

            if (!_context.Student.Any(r => r.Id == slot.StudentId))
            {
                ModelState.AddModelError("StudentId", $"Student {slot.StudentId} does not exist.");
            }

            var slotExist = _context.Slot.Any(x => x.RoomId == slot.RoomId && x.StartTime == slot.StartTime);
            if (!slotExist)
            {
                ModelState.AddModelError("StudentId", $"No slot exist in {slot.RoomId} at {slot.StartTime:dd-MM-yyyy HH:mm}");
            }

            var slotBooked = _context.Slot.Any(x => x.RoomId == slot.RoomId && x.StartTime == slot.StartTime && x.StudentId != null);
            if (slotBooked)
            {
                ModelState.AddModelError("StudentId", $"A student is already booked into this slot");
            }

            if (ModelState.IsValid)
            {
                // add student id to the slot in database
                _context.Slot.FirstOrDefault(x => x.RoomId == slot.RoomId && x.StartTime == slot.StartTime).StudentId = slot.StudentId;
                // track this slot to update
                _context.Slot.Update(_context.Slot.FirstOrDefault(x => x.RoomId == slot.RoomId && x.StartTime == slot.StartTime));

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(slot);
        }
    
        public IActionResult CancelBooking()
        {
            return View();
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelBooking([Bind("RoomId,StartTime,StudentId")] Slot slot)
        {
            if (!ModelState.IsValid) return View(slot);


            if (!_context.Room.Any(r => r.RoomId == slot.RoomId))
            {
                ModelState.AddModelError("RoomId", $"Room {slot.RoomId} does not exist.");
            }

            if (!_context.Student.Any(r => r.Id == slot.StudentId))
            {
                ModelState.AddModelError("StudentId", $"Student {slot.StudentId} does not exist.");
            }

            var slotExist = _context.Slot.Any(x => x.RoomId == slot.RoomId && x.StartTime == slot.StartTime);
            if (!slotExist)
            {
                ModelState.AddModelError("StartTime", $"No slot exist in {slot.RoomId} at {slot.StartTime:dd-MM-yyyy HH:mm}");
            }

            var slotEmpty = _context.Slot.Any(x => x.RoomId == slot.RoomId && x.StartTime == slot.StartTime && x.StudentId == null);
            if (slotEmpty)
            {
                ModelState.AddModelError("StudentId", $"No students booked into this slot");
            }

            var sameStudent = _context.Slot.Any(x => x.RoomId == slot.RoomId && x.StartTime == slot.StartTime && x.StudentId == slot.StudentId);
            if (!sameStudent)
            {
                ModelState.AddModelError("StudentId", $"The student ID for this slot does not match, cannot cancel this booking.");
            }

            if (ModelState.IsValid)
            {

                // remove the student id from the slot
                _context.Slot.FirstOrDefault(x => x.RoomId == slot.RoomId && x.StartTime == slot.StartTime).StudentId = null;
               
               _context.Slot.Update(_context.Slot.FirstOrDefault(x => x.RoomId == slot.RoomId && x.StartTime == slot.StartTime) );
               
                 await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(slot);
        }
    }
}
