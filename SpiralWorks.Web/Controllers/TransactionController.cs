using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpiralWorks.Interfaces;
using SpiralWorks.Web.Helpers;
using SpiralWorks.Web.Models;
using SpiralWorks.Model;
using Microsoft.AspNetCore.Routing;

namespace SpiralWorks.Web.Controllers
{
    [Authorize]
    public class TransactionController : BaseController
    {
        public TransactionController(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor) : base(uow, httpContextAccessor)
        {
        }

        public IActionResult Index(int? id)
        {
            var selectList = SelectListHelper.AccountList(_uow, _currentUser.UserId);

            ViewBag.AccountList = selectList;
            var list = new List<TransactionItemViewModel>();

            if (id == 0)
            {
                id = Convert.ToInt32(selectList.First().Value);
            }
            var items = _uow.Transactions.FindAll().Where(x => x.AccountId.Equals(id)).ToList();
            items.ForEach(x =>
            {
                var entity = new TransactionItemViewModel();
                x.CopyTo(entity);
                list.Add(entity);
            });

            var model = new TransactionViewModel()
            {
                AccountId = Convert.ToInt32(id),
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
        [HttpGet]
        public ActionResult Create(int? id)
        {

            int.TryParse(id.ToString(), out int accountId);
            var model = new TransactionItemViewModel()
            {
                AccountId = accountId,
                DateCreated = DateTime.Now
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TransactionItemViewModel model)
        {


            if (ModelState.IsValid)
            {
                var account = _uow.Accounts.FindById(model.AccountId);

                var transaction = new Model.Transaction()
                {
                    TransactionType = model.TransactionType,
                    AccountId = model.AccountId,
                    DateCreated = model.DateCreated,
                    Balance = account?.Balance ?? 0,

                };



                var recipient = new Model.Transaction();

                switch (model.TransactionType)
                {
                    case "DEP":
                        transaction.Debit = model.Amount;
                        transaction.Balance = transaction.Balance + transaction.Debit;
                        account.Balance = transaction.Balance;

                        break;
                    case "WIT":
                        transaction.Credit = model.Amount;
                        transaction.Balance = transaction.Balance - transaction.Credit;

                        account.Balance = transaction.Balance;

                        break;
                    case "TOA":
                    case "TSA":
                        transaction.Credit = model.Amount;
                        transaction.ToAccountId = model.ToAccountId;

                        recipient.AccountId = transaction.ToAccountId;
                        recipient.TransactionType = "TRF";
                        recipient.Debit = model.Amount;
                        recipient.DateCreated = DateTime.Now;

                        _uow.Transactions.Add(recipient);

                        break;
                    default:
                        break;
                }

                _uow.Transactions.Add(transaction);
                _uow.Accounts.Update(account);

                _uow.SaveChanges();

                return RedirectToAction("Index", new RouteValueDictionary(new { id = model.AccountId }));
            }
            return View(model);
        }
    }
}