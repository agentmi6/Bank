using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATM.Models
{
    public class qCash
    {
        public int Id { get; set; }

        [Required]
        public bool QuickCash { get; set; }

        public virtual CheckingAccount CheckingAccount { get; set; }
        public int CheckingAccountId { get; set; }

    }
}