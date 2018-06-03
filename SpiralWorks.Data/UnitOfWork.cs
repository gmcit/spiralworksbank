using Microsoft.EntityFrameworkCore;
using SpiralWorks.Interfaces;
using SpiralWorks.Model;
using System;
using System.Linq;
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
        public IRepository<Account> Accounts { get; }
        public IRepository<Transaction> Transactions { get; }

        public IRepository<UserAccount> UserAccounts { get; }

        public IRepository<User> Users { get; }
        public IRepository<UniqueNumber> UniqueNumbers { get; }

        public void SaveChanges()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var exceptionEntry = ex.Entries;

                StringBuilder sb = new StringBuilder();
                exceptionEntry.ToList().ForEach(x =>
                {
                    var values = Convert.ChangeType(x.Entity, x.GetType());
                    var dbEntry = x.GetDatabaseValues();
                    if (dbEntry != null)
                    {
                        sb.Append($"Unable to save changes. The {x.GetType()} was deleted by another user");
                    }
                    else
                    {
                        var dbValues = Convert.ChangeType(dbEntry.ToObject(), x.GetType()); //Todo: Iterate on the Types to get the Property
                                                                                            //via Reflection match the property names.
                    }

                });


                throw new Exception(sb.ToString());


            }

        }
    }
}