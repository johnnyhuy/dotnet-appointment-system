using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Data;
using Rmit.Asr.Application.Models;
using Rmit.Asr.Application.Models.Extensions;

namespace Rmit.Asr.Application.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Houses all the staff related functions.
    /// </summary>
    public class RoomController : Controller
    {
        private readonly ApplicationDataContext _context;

        public RoomController(ApplicationDataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Index showing a list of available rooms.
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AvailabilityIndex(DateTime day)
        {
            if (!ModelState.IsValid) return View();
            
            IQueryable<Room> rooms = _context.Room.GetAvailableRooms(day);

            return View(rooms);
        }
    }
}
