using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpiralWorks.Web.Models
{
    public class TransactionItemViewModel
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Amount { get; set; }
        public int SourceAccountId { get; set; }
        public string SourceAccountNumber { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
