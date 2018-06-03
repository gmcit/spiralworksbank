using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpiralWorks.Model
{
    [Table("UserAccount")]
    public class UserAccount
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int UserAccountId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Account")]
        public int AccountId { get; set; }
    }
}
