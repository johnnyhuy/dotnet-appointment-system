using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Data;
using Rmit.Asr.Application.Models;
using Rmit.Asr.Application.Models.ViewModels;

namespace Rmit.Asr.Application.Controllers.Api
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ApplicationDataContext _context;

        public RoomController(ApplicationDataContext context)
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
        /// <param name="value"></param>
        [HttpPost]
        public ActionResult Create([FromBody] dynamic value)
        {
            var room = new Room
            {
                RoomId = value.RoomId.Value
            };
            
            _context.Room.Add(room);
            _context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Update a room.
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{roomId}")]
        public ActionResult Put(string roomId, [FromBody] dynamic value)
        {
            Room room = _context.Room
                .FirstOrDefault(r => r.RoomId == roomId);
            
            if (room == null)
            {
                var error = new Error("Room does not exist.", HttpStatusCode.NotFound);
                return new JsonResult(error)
                {
                    StatusCode = error.StatusCode
                };
            }
            
            room.RoomId = value.RoomId.Value;
            
            _context.Room.Update(room);

            _context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Delete slot by room ID and start time.
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="startDate"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        [HttpDelete("{roomId}/{startDate}/{startTime}")]
        public ActionResult Delete(string roomId, DateTime startDate, DateTime startTime)
        {
            DateTime slotStartTime = startDate.Date.Add(startTime.TimeOfDay);
            
            Room room = _context.Room
                .FirstOrDefault(r => r.RoomId == roomId);
            
            if (room == null)
            {
                var error = new Error("Room does not exist.", HttpStatusCode.NotFound);
                return new JsonResult(error)
                {
                    StatusCode = error.StatusCode
                };
            }

            Slot slot = _context.Slot
                .FirstOrDefault(s => s.RoomId == roomId && s.StartTime == slotStartTime);
            
            if (slot == null)
            {
                var error = new Error("Slot does not exist.", HttpStatusCode.NotFound);
                return new JsonResult(error)
                {
                    StatusCode = error.StatusCode
                };
            }
            
            _context.Slot.Remove(slot);

            _context.SaveChanges();

            return Ok();
        }
    }
}