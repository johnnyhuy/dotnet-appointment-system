﻿using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Data;
using Rmit.Asr.Application.Models;
using Rmit.Asr.Application.Models.Extensions;
using Rmit.Asr.Application.Models.ViewModels;

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
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Staff.RoleName)]
        public IActionResult AvailabilityIndex()
        {
            return View(new AvailabilityRoom
            {
                Date = DateTime.Now.Date,
                AvailableRooms = _context.Room.GetAvailableRooms(DateTime.Now.Date)
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
            if (!ModelState.IsValid) return View(room);
            
            room.AvailableRooms =  _context.Room.GetAvailableRooms(room.Date);;

            return View(room);
        }
    }
}
