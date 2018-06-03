using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpiralWorks.Model
{
    [Table("Account")]
    public class Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string AccountName { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(500.0, Double.MaxValue, ErrorMessage = "Balance must be greater than or at least P 500 pesos")]
        public decimal Balance { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime DateCreated { get; set; }
    }

}
