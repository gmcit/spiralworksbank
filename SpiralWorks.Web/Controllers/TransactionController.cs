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
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpiralWorks.Model;

namespace SpiralWorks.Web.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        ITransactionService _transactionService;
        IAccountService _accountService;
        ISession _session;
        User _currentUser;
        public TransactionController(ITransactionService transactionService, IAccountService accountService, IHttpContextAccessor httpContextAccessor)
        {
            _transactionService = transactionService;
            _accountService = accountService;
            _session = httpContextAccessor.HttpContext.Session;
            _currentUser = _session.Get<User>("CurrentUser");
        }

        public IActionResult Index(int? id)
        {

            var list = new List<TransactionItemViewModel>();
            int.TryParse(id.ToString(), out int accountId);
            var items = _transactionService.GetTransactions(accountId);
            items.ForEach(x =>
            {
                var entity = new TransactionItemViewModel();
                x.CopyTo(entity);
                list.Add(entity);
            });

            var model = new TransactionViewModel()
            {
                AccountId = accountId,
                TransactionList = list
            };

            return View(model);
        }
        [HttpGet]
        public IActionResult List(int accountId)
        {
            var list = new List<TransactionItemViewModel>();

            if (accountId > 0)
            {
                var items = _transactionService.GetTransactions(accountId);

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
                TransactionType = "DEP",
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
                TimeSpan ts = new TimeSpan();


                var account = _accountService.GetAccount(model.AccountId);

                var transaction = new Transaction()
                {
                    TransactionType = model.TransactionType,
                    AccountId = model.AccountId,
                    DateCreated = model.DateCreated,
                    Balance = account?.Balance ?? 0,
                    RowVersion = ts.ToByteArray()
                };
                var recipient = new Transaction();

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

                        var recipientAccount = _accountService.GetAccount(model.ToAccountId);

                        recipient.AccountId = transaction.ToAccountId;
                        recipient.TransactionType = "TRF";
                        recipient.Debit = model.Amount;
                        recipient.ToAccountId = model.AccountId;
                        recipient.DateCreated = DateTime.Now;
                        recipient.Balance = recipientAccount.Balance + model.Amount;

                        _accountService.UpdateAccount(recipientAccount);

                        _transactionService.CreateTransaction(recipient);
                        break;

                    default:
                        break;
                }

                _transactionService.CreateTransaction(transaction);
                _accountService.UpdateAccount(account);



                return RedirectToAction("Index", new RouteValueDictionary(new { id = model.AccountId }));
            }
            return View(model);
        }

        public IActionResult AccountList(string transactionType)
        {
            SelectList selectList = null;
            if (transactionType == "TOA")
            {
                selectList = SelectListHelper.AccountList(_accountService, _currentUser.UserId);
            }
            else
            {
                selectList = SelectListHelper.OtherAccountList(_accountService, _currentUser.UserId);
            }
            return new JsonResult(selectList);
        }
    }
}