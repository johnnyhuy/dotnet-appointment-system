using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Rmit.Asr.Application.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rmit.Asr.Application.Controllers
{
    public class StaffMenuController : Controller
    {
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
        //public async Task<IActionResult> CreateSlot(Slot slot)
        //{
        //    return View(slot);
        //}

        public IActionResult ListStaff()
        {
            return View();
        }


        public IActionResult RemoveSlot()
        {
            return View();
        }


        public IActionResult RoomAvail()
        {
            return View();
        }

    }
}
