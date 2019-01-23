using System;
using System.Collections.Generic;
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
       
         // GET:
        public async Task<IActionResult> RoomAvail()
        {
            return View(await _context.Room.ToListAsync());
        }



        //public async Task<IActionResult> RemoveSlotAsync(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var slot = await _context.Slots.FirstOrDefaultAsync(x => x.RoomID == id );
        //    if (slot == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(slot);
        //    return View();
        //}



    }
}
