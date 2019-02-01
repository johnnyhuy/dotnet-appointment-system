using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Data;
using Rmit.Asr.Application.Models;
using Rmit.Asr.Application.Models.Extensions;
using Rmit.Asr.Application.Models.ViewModels;

namespace Rmit.Asr.Application.Controllers.Api
{
    [AllowAnonymous]
    [Route("api/room")]
    [ApiController]
    public class RoomApiController : ControllerBase
    {
        private readonly ApplicationDataContext _context;

        public RoomApiController(ApplicationDataContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Get all slots.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Room>> Index()
        {
            return _context.Room.ToList();
        }

        /// <summary>
        /// Create a room.
        /// </summary>
        /// <param name="room"></param>
        [HttpPost]
        public ActionResult Create([FromBody] CreateRoom room)
        {
            if (_context.Room.RoomExistsByName(room.Name))
            {
                ModelState.AddModelError("Name", "Room already exists.");
                return BadRequest(ModelState);
            }
            
            _context.Room.Add(room);
            _context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Update a room.
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="room"></param>
        /// <returns></returns>
        [HttpPut("{roomName}")]
        public ActionResult Put(string roomName, [FromBody] UpdateRoom room)
        {
            roomName = WebUtility.UrlDecode(roomName);
            
            Room updateRoom = _context.Room.FirstOrDefault(r => r.Name == roomName);
            if (updateRoom == null)
            {
                ModelState.AddModelError("Name", "Room does not exist.");
                return BadRequest(ModelState);
            }

            if (_context.Room.Any(r => r.Name == room.Name))
            {
                ModelState.AddModelError("Name", "Cannot update the room since the room already exists.");
                return BadRequest(ModelState);
            }

            updateRoom.Name = room.Name;
            
            _context.Room.Update(updateRoom);

            _context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Delete slot by room ID and start time.
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="startDate"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        [HttpDelete("{roomName}/{startDate}/{startTime}")]
        public ActionResult Delete(string roomName, DateTime startDate, DateTime startTime)
        {
            DateTime slotStartTime = startDate.Date.Add(startTime.TimeOfDay);

            roomName = WebUtility.UrlDecode(roomName);
            
            Room room = _context.Room
                .FirstOrDefault(r => r.Name == roomName);
            
            if (room == null)
            {
                ModelState.AddModelError("Name", "Room does not exist.");
                return BadRequest(ModelState);
            }

            Slot slot = _context.Slot
                .FirstOrDefault(s => s.RoomId == roomName && s.StartTime == slotStartTime);
            
            if (slot == null)
            {
                ModelState.AddModelError("Name", "Slot does not exist.");
                return BadRequest(ModelState);
            }
            
            _context.Slot.Remove(slot);

            _context.SaveChanges();

            return Ok();
        }
    }
}