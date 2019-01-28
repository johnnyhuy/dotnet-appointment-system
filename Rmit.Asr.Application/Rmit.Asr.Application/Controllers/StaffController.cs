using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rmit.Asr.Application.Data;
using Rmit.Asr.Application.Models;

namespace Rmit.Asr.Application.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Houses all the staff related functions.
    /// </summary>
    public class StaffController : Controller
    {
        private readonly ApplicationDataContext _context;

        public StaffController(ApplicationDataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Index show list of staff.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Staff.ToListAsync());
        }
        
        [HttpGet]
        public IActionResult AvailabilityIndex(DateTime date)
        {
            if (!ModelState.IsValid) return View();

            // gets all slots for that day that dont have a student booked into it
            // we know that staff cannot create a new slot if they have reached thier max bookings for that day
            // so all these slots must mean that these staff members are available
            IQueryable<Slot> availStaff = _context.Slot.Where(x => x.StartTime.Value.Date == date.Date && x.StudentId == null);

            return View(availStaff);
        }
    }
}
