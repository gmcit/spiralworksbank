using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpiralWorks.Interfaces;
using SpiralWorks.Model;
using SpiralWorks.Web.Helpers;

namespace SpiralWorks.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor) : base(uow, httpContextAccessor)
        {
        }

        public IActionResult Index()
        {
            var userAccounts = _uow.UserAccounts.Find(x => x.UserId.Equals(_currentUser.UserId)).ToList();
            var model = new List<Account>();
            userAccounts.ForEach(x =>
            {
                var entity = _uow.Accounts.FindById(x.AccountId);
                model.Add(entity);
            });

            return View(model);
        }
        public ActionResult Create()
        {
            var user = _session.Get<User>("CurrentUser");
            var dto = new UniqueNumber();
            _uow.UniqueNumbers.Add(dto);
            _uow.SaveChanges();
            var model = new Account()
            {
                AccountName = $"{user.FirstName} {user.LastName} #{_uow.UserAccounts.Find(x => x.UserId.Equals(user.UserId)).Count + 1}",
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
                _uow.Accounts.Add(model);
                _uow.SaveChanges();

                var dto = new UserAccount()
                {
                    AccountId = model.AccountId,
                    UserId = _currentUser.UserId
                };
                _uow.UserAccounts.Add(dto);
                _uow.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}