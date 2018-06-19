using Microsoft.AspNetCore.Mvc.Rendering;
using SpiralWorks.Interfaces;
using SpiralWorks.Model;
using System.Collections.Generic;
using System.Linq;

namespace SpiralWorks.Web.Helpers
{
    public class SelectListHelper
    {
        public static SelectList AccountList(IAccountService service, int userId)
        {
            var accounts = service.GetAccounts(userId);
            var list = new List<ListItem>();
            accounts.ToList().ForEach(x =>
            {
                list.Add(new ListItem() { Key = x.AccountId.ToString(), Value = $"{x.AccountNumber} - {x.AccountName}" });
            });
            return new SelectList(list, "Key", "Value");
        }
        public static SelectList OtherAccountList(IAccountService service, int userId)
        {
            var accounts = service.GetOtherAccounts(userId);

            var list = new List<ListItem>();
            accounts.ToList().ForEach(x =>
            {
                list.Add(new ListItem() { Key = x.AccountId.ToString(), Value = $"{x.AccountNumber} - {x.AccountName}" });
            });
            return new SelectList(list, "Key", "Value");
        }
        public static SelectList TransactionTypeList()
        {
            var list = new List<ListItem>();
            list.Add(new ListItem() { Key = "DEP", Value = "Deposit" });
            list.Add(new ListItem() { Key = "WIT", Value = "Withdrawal" });
            list.Add(new ListItem() { Key = "TOA", Value = "Transfer to your own Account" });
            list.Add(new ListItem() { Key = "TSA", Value = "Transfer to someone else's Account" });
            return new SelectList(list, "Key", "Value");
        }
    }
    public class ListItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
