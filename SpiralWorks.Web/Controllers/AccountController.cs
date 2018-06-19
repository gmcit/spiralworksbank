using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpiralWorks.Interfaces;
using SpiralWorks.Model;
using SpiralWorks.Web.Helpers;

namespace SpiralWorks.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        IAccountService _accountService;

        ISession _session;
        User _currentUser;

        public AccountController(IAccountService service, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = service;
            _session = httpContextAccessor.HttpContext.Session;
            _currentUser = _session.Get<User>("CurrentUser");
        }

        public IActionResult Index()
        {
            var model = _accountService.GetAccounts(_currentUser.UserId);

            return View(model);
        }
        public ActionResult Create()
        {

            var dto = _accountService.CreateAccountNumber();
            var accounts = _accountService.GetAccounts(_currentUser.UserId);

            var model = new Account()
            {
                AccountName = $"{_currentUser.FirstName} {_currentUser.LastName} #{accounts.Count + 1}",
                DateCreated = DateTime.Now,
                Balance = 0,
                AccountNumber = dto.AccountNumber

            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Create(Account model)
        {
            if (ModelState.IsValid)
            {
                _accountService.CreateAccount(model, _currentUser.UserId);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}