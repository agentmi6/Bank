using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATM.Models
{
    public class PrintStatementViewModel
    {
        public IEnumerable<CheckingAccount> _checkingAccounts { get; set; }
        public List<Transaction> _transactions { get; set; }

    }
}