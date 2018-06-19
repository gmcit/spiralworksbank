using SpiralWorks.Interfaces;
using SpiralWorks.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SpiralWorks.Data.Ado.Repositories
{
    public class TransactionRepository : IRepository<Transaction>
    {
        IDbContext _db;
        public TransactionRepository(IDbContext dbContext)
        {
            _db = dbContext;
        }

        public void Add(Transaction entity)
        {

            _db.CommandType = CommandType.Text;
            _db.CommandText = $"Insert into [Transaction](AccountId, TransactionType, Debit, Credit, ToAccountId, DateCreated, Balance ) " +
                 $"Values ({entity.AccountId},'{entity.TransactionType}',{entity.Debit},{entity.Credit},{entity.ToAccountId},GetDate(),{entity.Balance}); Select @@Identity as [Identity];";
            entity.TransactionId = _db.ExecuteNonQuery();

        }

        public void AddRange(List<Transaction> list)
        {

            list.ForEach(x =>
            {
                _db.CommandType = CommandType.Text;
                _db.CommandText = $"Insert into [Transaction](AccountId, TransactionType, Debit, Credit, ToAccountId, RowVersion, DateCreated, Balance ) " +
                $"Values ({x.AccountId},{x.TransactionType},{x.Debit},{x.Credit},{x.ToAccountId},{x.RowVersion} ,GetDate(),{x.Balance}); Select @@Identity as [Identity];";
                x.TransactionId = _db.ExecuteNonQuery();

            });

        }

        public void Commit()
        {
            _db.ExecuteNonQuery();
        }

        public void Delete(int id)
        {

            _db.CommandType = CommandType.Text;
            _db.CommandText = $"Delete From [Transaction] Where TransactionId={id}";
            _db.ExecuteNonQuery();

        }

        public void Delete(Transaction entity)
        {

            _db.CommandType = CommandType.Text;
            _db.CommandText = $"Delete From [Transaction] Where TransactionId={entity.TransactionId}";
            _db.ExecuteNonQuery();

        }

        public List<Transaction> Find(Func<Transaction, bool> match)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Transaction> FindAll()
        {
            IQueryable<Transaction> result = null;

            _db.CommandType = CommandType.Text;
            _db.CommandText = $"Select * from [Transaction]";
            result = _db.ExecuteToEntity<Transaction>().AsQueryable();

            return result;
        }

        public Transaction FindById(int id)
        {
            Transaction result = null;

            _db.CommandType = CommandType.Text;
            _db.CommandText = $"Select * from [Transaction] Where TransactionId={id}";
            result = _db.ExecuteToEntity<Transaction>().SingleOrDefault();

            return result;
        }

        public void Update(Transaction entity)
        {

            _db.CommandType = CommandType.Text;
            _db.CommandText = $"Update [Transaction] set AccountId={entity.AccountId}, TransactionType='{entity.TransactionType}', " +
                    $"Debit={entity.Debit}, Credit={entity.Credit}, ToAccountId={entity.ToAccountId}, RowVersion={entity.RowVersion}, " +
                    $"DateCreated={entity.DateCreated}, Balance={entity.Balance} where TransactionId={entity.TransactionId}";
            _db.ExecuteNonQuery();

        }
    }
}
