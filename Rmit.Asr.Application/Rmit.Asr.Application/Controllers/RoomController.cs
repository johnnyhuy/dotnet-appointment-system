using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class RoomController : Controller
    {
        private readonly ApplicationDataContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RoomController(ApplicationDataContext context, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// Index showing a list of available rooms.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Staff.RoleName)]
        public IActionResult AvailabilityIndex()
        {
            return View(new AvailabilityRoom
            {
                Date = _dateTimeProvider.Now().Date,
                AvailableRooms = _context.Room
                    .GetAvailableRooms(_dateTimeProvider.Now().Date)
                    .OrderBy(r => r.Name)
            });
        }

        /// <summary>
        /// Get the available rooms based on a date.
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("AvailabilityByDateIndex")]
        [Authorize(Roles = Staff.RoleName)]
        public IActionResult AvailabilityIndex([Bind("Date")]AvailabilityRoom room)
        {
            room.AvailableRooms = new List<Room>();
            
            if (!ModelState.IsValid) return View(room);

            if (room.Date >= _dateTimeProvider.Now())
            {
                room.AvailableRooms = _context.Room
                    .GetAvailableRooms(room.Date)
                    .OrderBy(r => r.Name);
            }

            return View(room);
        }
    }
}
