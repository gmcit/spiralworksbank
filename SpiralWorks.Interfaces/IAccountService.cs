using SpiralWorks.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpiralWorks.Interfaces
{
    public interface IAccountService
    {
        void CreateAccount(Account model,int userId);
        List<Account> GetAccounts(int userId);
        List<Account> GetOtherAccounts(int userId);
        Account GetAccount(int accountId);
        void UpdateAccount(Account model);
        UniqueNumber CreateAccountNumber();
    }
}
