using System.Collections.Generic;

namespace SpiralWorks.Web.Models
{
    public class TransactionViewModel
    {
        public int AccountId { get; set; }
        public IEnumerable<TransactionItemViewModel> TransactionList { get; set; }

    }
}
