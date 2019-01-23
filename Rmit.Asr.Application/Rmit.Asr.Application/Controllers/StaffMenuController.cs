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

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult CreateSlot()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult CreateSlot(Slot slot)
        //{
        //    return View(slot);
        //}

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


        public IActionResult ListStaff()
        {
            return View();
        }


        public async Task<IActionResult> RemoveSlotAsync(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var slot = await _context.Slots.FirstOrDefaultAsync(x => x.RoomID == id );

            //if (slot == null)
            //{
            //    return NotFound();
            //}

            //return View(slot);
            return View();

        }


        public IActionResult RoomAvail()
        {
            return View();
        }

    }
}
