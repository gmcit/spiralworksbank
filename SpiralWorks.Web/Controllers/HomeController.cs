using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpiralWorks.Interfaces;
using SpiralWorks.Model;
using SpiralWorks.Web.Helpers;
using SpiralWorks.Web.Models;

namespace SpiralWorks.Web.Controllers
{
    public class HomeController : Controller
    {
        ISession _session;
        User _currentUser;
        public HomeController( IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
            _currentUser = _session.Get<User>("CurrentUser");
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
