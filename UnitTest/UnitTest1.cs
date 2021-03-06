﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Models;
using ProsjektoppgaveNettbank.Controllers;
using DAL;
using BLL;
using System.Web.Mvc;



namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void adminRegisterCustomer()
        {
            // Arrange
            var controller = new BankController(new BankAdminBLL(new BankDALStub()));
            var newCust = new Customer()
            {
                nID = "12345678901",
                firstName = "Chad",
                lastName = "Thunder"
            };
            // Act
            var result = (RedirectToRouteResult)controller.AdminRegisterCustomer();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "AdminOverview");
        }  

        public void isAdminLoginCorrect()
        {
            // Arrange
            var controller = new BankController(new BankAdminBLL(new BankDALStub()));
            var adminlogg = new DbAdmin
            {
                ID = "1111",
                adminpassword =
            };

            // Act
            var actionResult = (RedirectToRouteResult)controller.AdminLogin();

            // Assert
            Assert.AreEqual(actionResult.RouteValues.Values.First(), "AdminOverview", "Bank");
        }

        /*    public void errorReport()
            {
                var controller = new BankController(new BankAdminBLL(new BankDALStub()));
                string text = "awdwad \n";

                var actionResult = (ViewResult)BankCustomerDAL.errorReport(text);

                Assert.IsTrue(actionResult.ViewData.ModelState.Count == 1);

            } */

        public void adminDeleteCustomer()
        {
            // Arrange
            var controller = new BankController(new BankAdminBLL(new BankDALStub()));

            // Act
            var actionResult = (ViewResult)controller.AdminDeleteCustomer(1);
            var resultat = (Customer)actionResult.Model;

            // Assert
            Assert.AreEqual(actionResult.ViewName, "");
        }
    }
}
