using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpiralWorks.Interfaces;
using SpiralWorks.Model;
using SpiralWorks.Web.Helpers;
using SpiralWorks.Web.Models;


namespace SpiralWorks.Web.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor) : base(uow, httpContextAccessor) { }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var status = LoginStatus.Failure;
                var user = Authenticate(model.Email, model.Password, out status);
                if (user != null)
                {
                    var claims = new List<Claim> { new Claim(ClaimTypes.Email, model.Email) };
                    var userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(principal);

                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Email / Password");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        public IActionResult Register(RegisterViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var dto = new User();
                model.CopyTo(dto);
                _uow.Users.Add(dto);
                _uow.SaveChanges();
                return RedirectToLocal(returnUrl);

            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {

            _session.Clear();
            await HttpContext.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        #region Helpers

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        private User Authenticate(string email, string password, out LoginStatus status)
        {

            status = LoginStatus.Failure;

            var user = _uow.Users.Find(x => x.Email.Equals(email) && x.Password.Equals(password)).FirstOrDefault();
            if (user != null)
            {
                status = LoginStatus.Success;
                _session.Set("CurrentUser", user);
            }

            return user;


        }


        #endregion
    }
}