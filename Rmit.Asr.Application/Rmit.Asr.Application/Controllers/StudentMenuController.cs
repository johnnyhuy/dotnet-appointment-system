using System;
using System.Collections.Generic;
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
            var availStaff = _context.Slot.Where(x => x.StartTime.Date == day.Date && x.StudentID == null);


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
        public async Task<IActionResult> MakeBooking([Bind("RoomID,StartTime,StudentID")] Slot slot)
        {
            if (!ModelState.IsValid) return View(slot);

            // A student can only mkae one booking per day
            var amountOfBookings = _context.Slot.Count(x => x.StartTime.Date == slot.StartTime.Date && x.StudentID == slot.StudentID);
            if(amountOfBookings != 0) // > 0 or >= 1
            {
                ModelState.AddModelError("StudentID", $"Student {slot.StudentID} has reached their maximum bookings for this day ({slot.StartTime.Date:dd-MM-yyyy})");
            }


            if (!_context.Room.Any(r => r.RoomID == slot.RoomID))
            {
                ModelState.AddModelError("RoomID", $"Room {slot.RoomID} does not exist.");
            }

            if (!_context.Student.Any(r => r.Id == slot.StudentID))
            {
                ModelState.AddModelError("StudentID", $"Student {slot.StudentID} does not exist.");
            }

            var slotExist = _context.Slot.Any(x => x.RoomID == slot.RoomID && x.StartTime == slot.StartTime);
            if (!slotExist)
            {
                ModelState.AddModelError("StudentID", $"No slot exist in {slot.RoomID} at {slot.StartTime:dd-MM-yyyy HH:mm}");
            }

            var slotBooked = _context.Slot.Any(x => x.RoomID == slot.RoomID && x.StartTime == slot.StartTime && x.StudentID == null);
            if (!slotBooked)
            {
                ModelState.AddModelError("StudentID", $"A student is already booked into this slot");
            }

            if (ModelState.IsValid)
            {
                // make sure the staff id is added to the slot before updating 
                // to prevent the slot in the database being overridden with no staff id
                slot.StaffID = _context.Slot.FirstOrDefault(x => x.RoomID == slot.RoomID && x.StartTime == slot.StartTime).StaffID;
               
                 _context.Slot.Update(slot);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(slot);
        }
    }
}
