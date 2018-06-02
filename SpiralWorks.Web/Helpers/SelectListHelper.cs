using Microsoft.AspNetCore.Mvc.Rendering;
using SpiralWorks.Interfaces;
using SpiralWorks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpiralWorks.Web.Helpers
{
    public class SelectListHelper
    {
        public static SelectList AccountList(IUnitOfWork uow, int userId)
        {
            var userAccounts = uow.UserAccounts.Find(x => x.UserId.Equals(userId)).ToList();
            var model = new List<Account>();
            userAccounts.ForEach(x =>
            {
                var entity = uow.Accounts.FindById(x.AccountId);
                model.Add(entity);
            });

            var list = new List<ListItem>();
            model.ToList().ForEach(x =>
            {
                list.Add(new ListItem() { Key = x.AccountId.ToString(), Value = $"{x.AccountNumber} - {x.AccountName}" });
            });
            return new SelectList(list, "Key", "Value");
        }
    }
    public class ListItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
