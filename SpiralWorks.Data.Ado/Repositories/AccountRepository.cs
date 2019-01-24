using SpiralWorks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using SpiralWorks.Interfaces;
using System.Data;

namespace SpiralWorks.Data.Ado.Repositories
{
    public class AccountRepository : IRepository<Account>
    {
        IDbContext _db;
        public AccountRepository(IDbContext dbContext)
        {
            _db = dbContext;
        }

        public void Add(Account entity)
        {

                _db.CommandType = CommandType.Text;
                _db.CommandText = $"Insert into Account(AccountNumber, AccountName, Balance, DateCreated) " +
                 $"Values ('{entity.AccountNumber}','{entity.AccountName}',{entity.Balance},GetDate()); Select @@Identity as [Identity];";


        }

        public void AddRange(List<Account> list)
        {

                list.ForEach(x =>
                {
                    _db.CommandType = CommandType.Text;
                    _db.CommandText = $"Insert into Account(AccountNumber, AccountName, Balance,  DateCreated) " +
                    $"Values ('{x.AccountNumber}','{x.AccountName}',{x.Balance}, GateDate()); Select @@Identity as [Identity];";


                });

        }

        public void Commit()
        {
            _db.ExecuteNonQuery();
        }

        public void Delete(int id)
        {

            _db.CommandType = CommandType.Text;
            _db.CommandText = $"Delete From Account Where AccountId={id}; " +
                $"Select @@RowCount as [RowCount]";
            _db.ExecuteNonQuery();

        }

        public void Delete(Account entity)
        {

            _db.CommandType = CommandType.Text;
            _db.CommandText = $"Delete From Account Where AccountId={entity.AccountId}; " +
                $"Select @@RowCount as [RowCount]";
            _db.ExecuteNonQuery();

        }

        public List<Account> Find(Func<Account, bool> match)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Account> FindAll()
        {
            IQueryable<Account> result = null;

            _db.CommandType = CommandType.Text;
            _db.CommandText = $"Select * from Account";
            result = _db.ExecuteToEntity<Account>().AsQueryable();

            return result;
        }

        public Account FindById(int id)
        {
            Account result = null;

            _db.CommandType = CommandType.Text;
            _db.CommandText = $"Select * from Account Where AccountId={id}";
            result = _db.ExecuteToEntity<Account>().SingleOrDefault();

            return result;
        }

        public void Update(Account entity)
        {
            _db.CommandType = CommandType.Text;
            _db.CommandText = $"Update Account set AccountNumber='{entity.AccountNumber}', " +
                    $"AccountName='{entity.AccountName}', Balance={entity.Balance} where AccountId={entity.AccountId};" +
                    $" Select @@RowCount as [RowCount]";


        }
    }
}
