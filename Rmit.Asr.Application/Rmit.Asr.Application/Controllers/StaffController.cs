using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rmit.Asr.Application.Data;

namespace Rmit.Asr.Application.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Houses all the staff related functions.
    /// </summary>
    public class StaffController : Controller
    {
        private readonly ApplicationDataContext _context;

        public StaffController(ApplicationDataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Index show list of staff.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Staff.ToListAsync());
        }
    }
}
