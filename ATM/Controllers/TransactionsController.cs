using ATM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATM.Controllers
{
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Deposit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Deposit(Transaction ta)
        {
            var currentUserId = User.Identity.GetUserId();

            var checkingAccountID =
                  (from c in db.CheckingAccounts
                   where c.ApplicationUserID == currentUserId
                   select c.Id).First();

            var checkingAccount =
                 (from c in db.CheckingAccounts
                  where c.ApplicationUserID == currentUserId
                  select c).First();

            var checkingAccountBalance =
                (from c in db.CheckingAccounts
                 where c.ApplicationUserID == currentUserId
                 select c.Balance).First();

            ta.CheckingAccountId = checkingAccountID;
            checkingAccount.Balance += ta.Amount;
            db.Entry(checkingAccount).State = EntityState.Modified;

            db.Transactions.Add(ta);
            db.SaveChanges();


            return RedirectToAction("Details", "CheckingAccount", new { checkingAccountId = checkingAccountID });
        }

        public ActionResult Withdrawal()
        {
            var _userId = User.Identity.GetUserId();
            var checkingAccount =
            (from c in db.CheckingAccounts
             where c.ApplicationUserID == _userId
             select c).First();

            ViewBag.CheckingAccountBalance = checkingAccount.Balance;

            return View();
        }

        [HttpPost]
        public ActionResult Withdrawal(Transaction ta)
        {
            var _userId = User.Identity.GetUserId();

            var checkingAccount =
                (from c in db.CheckingAccounts
                 where c.ApplicationUserID == _userId
                 select c).First();

            var checkingAccountID =
                (from c in db.CheckingAccounts
                 where c.ApplicationUserID == _userId
                 select c).First().Id;


            ViewBag.CheckingAccountID = checkingAccountID;


            if (ModelState.IsValid)
            {
                ta.CheckingAccountId = checkingAccountID;

                if (checkingAccount.Balance < ta.Amount)
                {
                    ViewBag.Error = "Insufficient funds";
                    ViewBag.CheckingAccountBalance = checkingAccount.Balance;
                    return View(ta);
                }
                else
                {
                    checkingAccount.Balance -= ta.Amount;
                }

                db.Entry(checkingAccount).State = EntityState.Modified;
                db.Transactions.Add(ta);
                db.SaveChanges();

                return RedirectToAction("Details", "CheckingAccount", new { checkingAccountId = ViewBag.CheckingAccountID });
            }

            return View();
        }

        public ActionResult QuickCash(int checkingAccountId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult QuickCash(qCash cash)
        {
            var _userId = User.Identity.GetUserId();
            var checkingAccount = db.CheckingAccounts.Where(x => x.ApplicationUserID == _userId).First();
            var checkingAccountId = db.CheckingAccounts.Where(x => x.ApplicationUserID == _userId).First().Id;

            if (ModelState.IsValid)
            {
                if (checkingAccount.Balance >= 100)
                {
                    checkingAccount.Balance -= 100;
                    db.fastTransactions.Add(cash);
                    db.Entry(checkingAccount).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Message = "Your quick-cash transaction was successful.";
                    return RedirectToAction("Details", "CheckingAccount", new { checkingAccountId = checkingAccountId });
                }
                else
                {
                    ViewBag.Error = "Insufficient funds... you need at least $100 to make a quick cash transaction.";
                    return View(cash);
                }
            }
            return View(cash);
        }

        public ActionResult TransferFunds(int checkingAccountId)
        {
            var _userId = User.Identity.GetUserId();
            var checkingAccount = db.CheckingAccounts.Where(x => x.ApplicationUserID == _userId).First();

            ViewBag.AvailableFunds = checkingAccount.Balance;

            return View();
        }

        [HttpPost]
        public ActionResult TransferFunds(Transfer transfer)
        {
            var _userId = User.Identity.GetUserId();
            var myCheckingAccount = db.CheckingAccounts.Where(x => x.ApplicationUserID == _userId).First();


            ViewBag.AvailableFunds = myCheckingAccount.Balance;

            if (ModelState.IsValid)
            {

                var transferToAccount = transfer.CheckingAccountNumberToTransfer;

                if (db.CheckingAccounts.Any(x => x.AccountNumber == transferToAccount))
                {

                    var otherCheckingAccount = db.CheckingAccounts.Where(x => x.AccountNumber == transferToAccount).First();
                    var checkingAccountNumber = db.CheckingAccounts.Where(x => x.AccountNumber == transferToAccount).First().AccountNumber;

                    var transferFunds = transfer.TransferAmount;
                    //myCheckingAccount.Balance -= transferFunds;

                    if (transfer.TransferAmount > myCheckingAccount.Balance)
                    {
                        ViewBag.InsufficientFunds = "Insufficent funds to transfer! Check your available funds...";
                        return View(transfer);
                    }
                    myCheckingAccount.Balance -= transferFunds;
                    otherCheckingAccount.Balance += transferFunds;
                    db.Entry(myCheckingAccount).State = EntityState.Modified;
                    db.Entry(otherCheckingAccount).State = EntityState.Modified;
                    //db.TransferFunds.Add(transfer);
                    db.SaveChanges();
                    ViewBag.TransferComplete = "Transfer completed.";
                    ViewBag.AvailableFunds = myCheckingAccount.Balance;

                    return View(transfer);
                }

            }

            return View();
        }

        public ActionResult PrintStatement()
        {
            var currentUser = User.Identity.GetUserId();
            var checkingAccountId = db.CheckingAccounts.Where(x => x.ApplicationUserID == currentUser).First().Id;

            ViewBag.CheckingAccountId = checkingAccountId;

            var accounts = db.CheckingAccounts.Where(x => x.ApplicationUserID == currentUser).ToList();
            var transactionsByAccount = db.Transactions.Where(x=>x.CheckingAccountId == checkingAccountId).ToList();

            var model = new PrintStatementViewModel
            {
                _transactions = transactionsByAccount,
                _checkingAccounts = accounts
            };
       
            return View(model);
        }
    }
}