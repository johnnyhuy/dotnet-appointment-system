using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Rmit.Asr.Application.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Need an assignment demonstration to be booked at a certain time? So no more!";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Interested in improving this application? Get in touch!";

            return View();
        }

        public IActionResult Faq()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
