using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpiralWorks.Interfaces;
using SpiralWorks.Web.Helpers;
using SpiralWorks.Web.Models;

namespace SpiralWorks.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor) : base(uow, httpContextAccessor)
        {
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated && _currentUser != null)
            {
                return RedirectToAction("Index", "Account");
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "SpiralWorks Sample App";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Louie Gomez";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
