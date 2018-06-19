using SpiralWorks.Data.Ado.Repositories;
using SpiralWorks.Interfaces;
using SpiralWorks.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpiralWorks.Data.Ado
{
    public class UnitOfWork : IUnitOfWork
    {
        IDbContext _dbContext;
        public UnitOfWork(IDbContext dbContext)
        {
            _dbContext = dbContext;
            Accounts = new AccountRepository(_dbContext);
            Transactions = new TransactionRepository(_dbContext);
            UserAccounts = new UserAccountRepository(_dbContext);
            Users = new UserRepository(dbContext);
            UniqueNumbers = new UniqueNumberRepository(_dbContext);
        }

        public IRepository<Account> Accounts { get; }

        public IRepository<Transaction> Transactions { get; }

        public IRepository<UserAccount> UserAccounts { get; }

        public IRepository<User> Users { get; }

        public IRepository<UniqueNumber> UniqueNumbers { get; }

        public void SaveChanges()
        {
            _dbContext.ExecuteNonQuery();
        }
    }
}
