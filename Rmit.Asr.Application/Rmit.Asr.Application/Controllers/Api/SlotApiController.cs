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
    public class SlotApiController : ControllerBase
    {
        private readonly ApplicationDataContext _context;

        public SlotApiController(ApplicationDataContext context)
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
        [HttpGet("student/{StudentId}")]
        public ActionResult<IEnumerable<Slot>> StudentIndex(Student student)
        {
            return _context.Slot
                .Include(s => s.Room)
                .Include(s => s.Staff)
                .Include(s => s.Student)
                .Where(s => s.Student.StudentId == student.StudentId)
                .ToList();
        }
        
        /// <summary>
        /// Get booked staff slots.
        /// </summary>
        /// <returns></returns>
        [HttpGet("staff/{StaffId}")]
        public ActionResult<IEnumerable<Slot>> StaffIndex(Staff staff)
        {
            return _context.Slot
                .Include(s => s.Room)
                .Include(s => s.Staff)
                .Include(s => s.Student)
                .Where(s => s.Staff.StaffId == staff.StaffId)
                .ToList();
        }

        /// <summary>
        /// Update a slot.
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="startDate"></param>
        /// <param name="startTime"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        [HttpPut("{roomName}/{startDate}/{startTime}")]
        public ActionResult Put(string roomName, DateTime startDate, DateTime startTime, [FromBody] Slot slot)
        {
            DateTime slotStartTime = startDate.Date.Add(startTime.TimeOfDay);
            slot.StartTime = slotStartTime;

            Student student = _context.Student.FirstOrDefault(s => s.StudentId == slot.StudentId);
            
            if (student == null && !string.IsNullOrEmpty(slot.StudentId))
            {
                var error = new Error("Student does not exist.", HttpStatusCode.NotFound);
                return new JsonResult(error)
                {
                    StatusCode = error.StatusCode
                };
            }

            Room room = _context.Room
                .FirstOrDefault(r => r.Name == roomName);
            
            if (room == null)
            {
                var error = new Error("Room does not exist.", HttpStatusCode.NotFound);
                return new JsonResult(error)
                {
                    StatusCode = error.StatusCode
                };
            }
            
            Slot updateSlot = _context.Slot
                .FirstOrDefault(s => s.RoomId == room.Id && s.StartTime == slotStartTime);
            
            if (updateSlot == null)
            {
                var error = new Error("Slot does not exist.", HttpStatusCode.NotFound);
                return new JsonResult(error)
                {
                    StatusCode = error.StatusCode
                };
            }

            updateSlot.StudentId = student?.StudentId;

            _context.Slot.Update(slot);

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
            
            Room room = _context.Room
                .FirstOrDefault(r => r.Name == roomName);
            
            if (room == null)
            {
                var error = new Error("Room does not exist.", HttpStatusCode.NotFound);
                return new JsonResult(error)
                {
                    StatusCode = error.StatusCode
                };
            }

            Slot slot = _context.Slot
                .Include(s => s.Room)
                .FirstOrDefault(s => s.Room.Name == roomName && s.StartTime == slotStartTime);
            
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