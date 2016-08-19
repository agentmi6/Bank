using ATM.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ATM.Controllers
{
    public class CheckingAccountController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        
      
        public ActionResult Details(int? checkingAccountId)
        {
            if (checkingAccountId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckingAccount account = db.CheckingAccounts.Find(checkingAccountId);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }             
    }
}
