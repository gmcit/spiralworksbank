using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SpiralWorks.Model
{
    [Table("Transaction")]
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int TransactionId { get; set; }
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public string Description {get;set;}
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        [ForeignKey("Account")]
        public int SourceAccountId { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
