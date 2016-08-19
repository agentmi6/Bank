using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATM.Models
{
    public class UserTransactionsViewModel
    {
        public CheckingAccount accounts { get; set; }
        public Transaction transaction { get; set; }


    }
}