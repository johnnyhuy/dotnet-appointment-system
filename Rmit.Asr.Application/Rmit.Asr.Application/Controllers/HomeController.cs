﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Rmit.Asr.Application.Models;

namespace Rmit.Asr.Application.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Web Development Technologies -  Assginement 2";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact Us.";

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
