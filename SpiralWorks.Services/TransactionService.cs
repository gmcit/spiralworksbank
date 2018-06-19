using SpiralWorks.Interfaces;
using System;
using System.Collections.Generic;
using SpiralWorks.Model;
using System.Linq;

namespace SpiralWorks.Services
{
    public class TransactionService : ITransactionService
    {
        IUnitOfWork _uow;

        public TransactionService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void CreateTransaction(Transaction model)
        {
            try
            {
                _uow.Transactions.Add(model);
                _uow.SaveChanges();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }

        }

        public Transaction GetTransaction(int transactionId)
        {
            try
            {
              return _uow.Transactions.FindById(transactionId);

            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public List<Transaction> GetTransactions(int accountId)
        {

            try
            {
               var transactions = _uow.Transactions.FindAll().Where(x => x.AccountId.Equals(accountId));
                return transactions.ToList();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
    }
}
