using SpiralWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpiralWorks.Web.Attributes
{
    public class CheckBalanceAttribute : ValidationAttribute
    {

        public CheckBalanceAttribute() { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var amount = (decimal)value;

            var acct = validationContext.ObjectType.GetProperty("AccountId");
            var tran = validationContext.ObjectType.GetProperty("TransactionType");

            if (acct == null) throw new ArgumentException("Property with this name not found");
            if (tran == null) throw new ArgumentException("Property with this name not found");

            var accountId = (int)acct.GetValue(validationContext.ObjectInstance);
            var tranType = (string)tran.GetValue(validationContext.ObjectInstance);

            var _uow = (IUnitOfWork)validationContext.GetService(typeof(IUnitOfWork));

            var dto = _uow.Accounts.FindById(accountId);

            if (tranType.Equals("WIT") || tranType.Equals("WIT") || tranType.Equals("TOA") || tranType.Equals("TSA"))
                if (amount > dto.Balance)
                {
                    ErrorMessage = string.Format("Amount being {0} is greater than your current balance", tranType.Equals("WIT") ? "withdrawn" : "transferred");

                    return new ValidationResult(ErrorMessage, new string[] { validationContext.MemberName });

                }
            return ValidationResult.Success;
        }
    }
}
