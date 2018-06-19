using System;
using System.Collections.Generic;
using System.Text;
using SpiralWorks.Model;

namespace SpiralWorks.Interfaces
{
    public interface ITransactionService
    {
        void CreateTransaction(Transaction model);
        List<Transaction> GetTransactions(int accountId);
        Transaction GetTransaction(int transactionId);

    }
}
