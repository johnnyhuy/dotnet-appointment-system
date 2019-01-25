using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rmit.Asr.Application.Controllers
{
    public class StudentMenuController : Controller
    {
        private readonly ApplicationDataContext _context;

        public StudentMenuController(ApplicationDataContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // GET:
        public IActionResult ListStudents()
        {
            return View(_context.Student.ToList() );
        }

    }
}
