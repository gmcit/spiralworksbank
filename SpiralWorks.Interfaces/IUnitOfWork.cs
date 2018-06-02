using SpiralWorks.Model;
using System;

namespace SpiralWorks.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Account> Accounts { get; }
        IRepository<Transaction> Transactions { get;  }
        IRepository<UserAccount> UserAccounts { get; }
        IRepository<User> Users { get; }
        IRepository<UniqueNumber> UniqueNumbers { get; }
        void SaveChanges();
    }
}
