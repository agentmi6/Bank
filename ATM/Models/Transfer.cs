using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATM.Models
{
    public class Transfer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Amount to transfer")]
        public decimal TransferAmount { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "The account number is a 10 digit number.")]
        [Display(Name = "Account Number to transfer to")]
        public string CheckingAccountNumberToTransfer { get; set; }

        [Required]
        [Display(Name = "Confirm?")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You need to check the box so you can confirm the transfer.")]
        public bool ConfirmTransfer { get; set; }

        public virtual CheckingAccount CheckingAccount{ get; set; }
        public int CheckingAccountID { get; set; }
    }
}