using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpiralWorks.Interfaces;
using SpiralWorks.Web.Helpers;
using SpiralWorks.Web.Models;

namespace SpiralWorks.Web.Controllers
{
    [Authorize]
    public class TransactionController : BaseController
    {
        public TransactionController(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor) : base(uow, httpContextAccessor)
        {
        }

        public IActionResult Index(int accountId = 0)
        {
            var selectList = SelectListHelper.AccountList(_uow, _currentUser.UserId);

            ViewBag.AccountList = selectList;
            var list = new List<TransactionItemViewModel>();
            var id = Convert.ToInt32(selectList.Skip(1).First().Value);
            if (accountId == 0)
            {
                var items = _uow.Transactions.FindAll().Where(x => x.AccountId.Equals(id)).ToList();
                items.ForEach(x =>
                {
                    var entity = new TransactionItemViewModel();
                    x.CopyTo(entity);
                    list.Add(entity);
                });
            }

            var model = new TransactionViewModel()
            {
                AccountId = id,
                TransactionList = list
            };

            return View(model);
        }
        [HttpGet]
        public IActionResult List(int AccountId)
        {
            var list = new List<TransactionItemViewModel>();

            if (AccountId > 0)
            {
                var items = _uow.Transactions.FindAll().Where(x => x.AccountId.Equals(AccountId)).ToList();
                items.ForEach(x =>
                {
                    var entity = new TransactionItemViewModel();
                    x.CopyTo(entity);
                    list.Add(entity);
                });
            }
            return PartialView("TransactionList", list);
        }
    }
}