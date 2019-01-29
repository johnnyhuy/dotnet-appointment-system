using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rmit.Asr.Application.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDataContext _context;

        public StudentController(ApplicationDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_context.Student);
        }
    }
}
