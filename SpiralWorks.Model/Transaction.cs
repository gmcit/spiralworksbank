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
        [Required]
        public string TransactionType {get;set;}
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        [ForeignKey("Account")]
        public int ToAccountId { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
