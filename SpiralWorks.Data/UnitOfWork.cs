using Microsoft.EntityFrameworkCore;
using SpiralWorks.Interfaces;
using SpiralWorks.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpiralWorks.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext _dbContext;
        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;

            Accounts = new Repository<Account>(_dbContext);
            Transactions = new Repository<Transaction>(_dbContext);
            UserAccounts = new Repository<UserAccount>(_dbContext);
            Users = new Repository<User>(_dbContext);
            UniqueNumbers = new Repository<UniqueNumber>(_dbContext);
        }
        public IRepository<Account> Accounts { get ; }
        public IRepository<Transaction> Transactions { get ; }

        public IRepository<UserAccount> UserAccounts { get; }

        public IRepository<User> Users { get; }
        public IRepository<UniqueNumber> UniqueNumbers { get; }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
