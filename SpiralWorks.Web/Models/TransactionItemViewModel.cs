using SpiralWorks.Web.Attributes;
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
        [Required]
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        [Required]
        public string TransactionType { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        [Required]
        [CheckBalance]
        [DataType(DataType.Currency)]
        [Range(1.0, Double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
        public int ToAccountId { get; set; }
        public string ToAccountNumber { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
    }
}
