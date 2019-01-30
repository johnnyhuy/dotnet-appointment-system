using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rmit.Asr.Application.Data;
using Rmit.Asr.Application.Models;

namespace Rmit.Asr.Application.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        private ApplicationDataContext _context;

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
        [HttpGet("booked")]
        public ActionResult<IEnumerable<Slot>> BookedIndex()
        {
            return _context.Slot
                .Include(s => s.Room)
                .Include(s => s.Staff)
                .Include(s => s.Student)
                .Where(s => !string.IsNullOrEmpty(s.StudentId))
                .ToList();
        }
        
        /// <summary>
        /// Get booked student slots.
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

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}