using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpiralWorks.Web.Models
{
    public class TransactionViewModel
    {
        public int AccountId { get; set; }
        public IEnumerable<TransactionItemViewModel> TransactionList { get; set; }

    }
}
