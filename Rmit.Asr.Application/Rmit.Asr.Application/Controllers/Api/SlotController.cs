using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rmit.Asr.Application.Data;
using Rmit.Asr.Application.Models;
using Rmit.Asr.Application.Models.ViewModels;

namespace Rmit.Asr.Application.Controllers.Api
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        private readonly ApplicationDataContext _context;

        public SlotController(ApplicationDataContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Get all slots.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Slot>> Index()
        {
            return _context.Slot
                .Include(s => s.Room)
                .Include(s => s.Staff)
                .Include(s => s.Student)
                .ToList();
        }

        /// <summary>
        /// Get booked student slots.
        /// </summary>
        /// <returns></returns>
        [HttpGet("student/{studentId}")]
        public ActionResult<IEnumerable<Slot>> StudentIndex(string studentId)
        {
            return _context.Slot
                .Include(s => s.Room)
                .Include(s => s.Staff)
                .Include(s => s.Student)
                .Where(s => s.Student.StudentId == studentId)
                .ToList();
        }
        
        /// <summary>
        /// Get booked staff slots.
        /// </summary>
        /// <returns></returns>
        [HttpGet("staff/{staffId}")]
        public ActionResult<IEnumerable<Slot>> StaffIndex(string staffId)
        {
            return _context.Slot
                .Include(s => s.Room)
                .Include(s => s.Staff)
                .Include(s => s.Student)
                .Where(s => s.Staff.StaffId == staffId)
                .ToList();
        }

        [HttpPost]
        public void Post([FromBody] dynamic value)
        {
        }

        [HttpPut("{roomId}/{startDate}/{startTime}")]
        public ActionResult Put(string roomId, DateTime startDate, DateTime startTime, [FromBody] dynamic value)
        {
            DateTime slotStartTime = startDate.Date.Add(startTime.TimeOfDay);
            string studentId = value.StudentId.Value;

            Student student = _context.Student.FirstOrDefault(s => s.StudentId == studentId);
            
            if (student == null && !string.IsNullOrEmpty(studentId))
            {
                var error = new Error("Student does not exist.", HttpStatusCode.NotFound);
                return new JsonResult(error)
                {
                    StatusCode = error.StatusCode
                };
            }

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
            
            slot.StudentId = value.StudentId.Value;
            
            _context.Slot.Update(slot);

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