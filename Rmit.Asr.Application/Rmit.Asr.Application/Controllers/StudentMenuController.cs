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




    }
}
