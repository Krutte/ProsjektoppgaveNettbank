﻿using ProsjektoppgaveNettbank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using ProsjektoppgaveNettbank;

namespace ProsjektoppgaveNettbank.Controllers
{
    public class BankController : Controller
    {
        public ActionResult BankIndex()
        {
            if (Session["LoggedIn"] == null)
            {
                Session["LoggedIn"] = false;
                ViewBag.LoggedIn = false;
            }
            else
            {
                ViewBag.LoggedIn = (bool)Session["LoggedIn"];
            }
            return View();
        }

        [HttpPost]
        public ActionResult BankIndex(Customer customer)
        {
            var bankBLL = new BankDAL();
            if (bankBLL.isLoginCorrect(customer))
            {
                Session["LoggedIn"] = true;
                ViewBag.LoggedIn = true;
                Session["NID"] = customer.nID;
                return RedirectToAction("AccountOverview", "Bank");
            }
            Session["LoggedIn"] = false;
            ViewBag.LoggedIn = false;
            return View();
        }

        public ActionResult RegisterCustomer()
        {
            var bankBLL = new BankDAL();
            bankBLL.populateDatabase();
            return View();
        }

        public ActionResult AccountOverview()
        {
            if (Session["LoggedIn"] != null)
            {
                bool LoggedIn = (bool)Session["LoggedIn"];
                if (LoggedIn)
                {
                    return View();
                }
            }
            return RedirectToAction("BankIndex", "Bank");
        }


        public ActionResult EditPayment(string id)
        {
            if (Session["LoggedIn"] != null)
            {
                bool LoggedIn = (bool)Session["LoggedIn"];
                if (LoggedIn)
                {
                    var bankBLL = new BankDAL();
                    RegisteredPayment payment = bankBLL.findRegisteredPayment(Convert.ToInt32(id));
                    return View(payment);
                }
            }
            return RedirectToAction("BankIndex", "Bank");
        }

        [HttpPost]
        public ActionResult EditPayment(RegisteredPayment registeredPayment)
        {
            var bankBLL = new BankDAL();
            if(!bankBLL.editPayment(registeredPayment))
                return View(registeredPayment);

            return RedirectToAction("AccountOverview", "Bank");
        }

        public ActionResult RegisterSinglePayment(string id)
        {
            if (Session["LoggedIn"] != null)
            {
                bool LoggedIn = (bool)Session["LoggedIn"];
                if (LoggedIn)
                {
                    Session["accountNumber"] = id;
                    return View();
                }
            }
            return RedirectToAction("BankIndex", "Bank");
        }


        [HttpPost]
        public ActionResult RegisterSinglePayment(RegisteredPayment registeredPayment)
        {
            if (Session["LoggedIn"] == null || Session["accountNumber"] == null)
            {
                return RedirectToAction("BankIndex", "Bank");
            }
            registeredPayment.accountNumberFrom = (string) Session["accountNumber"];
            var bankBLL = new BankDAL();
            bankBLL.registerPayment(registeredPayment);
            ViewBag.AccountNumber = registeredPayment.accountNumberFrom;
            return RedirectToAction("AccountOverview", "Bank");
        }

        public string DeletePayment(string id)
        {
            var bankBLL = new BankDAL();
            string currentAccount = bankBLL.getRegisteredPaymentAccount(Convert.ToInt32(id));

            bankBLL.deletePayment(Convert.ToInt32(id));
            string allRegisteredPayments = getRegisteredPayments(currentAccount);

            return allRegisteredPayments;
        }

        public string getCustomerAccounts()
        {
            var bankBLL = new BankDAL();
            List<Account> allCustomerAccounts = bankBLL.getCustomerAccounts((String)Session["NID"]);
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(allCustomerAccounts);
            return json;
        }

        public string getRegisteredPayments(string id)
        {
            var bankBLL = new BankDAL();
            List<RegisteredPayment> allRegisteredPayments = bankBLL.getRegisteredPayments(id);
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(allRegisteredPayments);
            return json;
        }

        public ActionResult LogOut()
        {
            Session["LoggedIn"] = null;
            ViewBag.LoggedIn = null;
            Session["NID"] = null;
            return RedirectToAction("BankIndex", "Bank");
        }

    }
}