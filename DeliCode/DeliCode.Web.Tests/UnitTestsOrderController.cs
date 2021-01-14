using DeliCode.Library.Models;
using DeliCode.Web.Controllers;
using DeliCode.Web.Models;
using DeliCode.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliCode.Web.Tests
{
    public class UnitTestsOrderController
    {
        readonly OrderController orderController;
        public UnitTestsOrderController()
        {
            orderController = new OrderController();
        }

        [Fact]
        public void Index_ShouldReturnViewResultWithModelCart()
        {
            OrderSummary.Cart = new Cart();
            var action = orderController.Index();
            ViewResult result = action as ViewResult;
            var actual = result.Model as Cart;

            Assert.IsType<Cart>(actual);
        }
        [Fact]
        public void ConfirmOrder_ShouldReturnViewResultWithModelCart()
        {
            OrderSummary.Cart = new Cart();
            var action = orderController.ConfirmOrder("", "");
            ViewResult result = action as ViewResult;
            var actual = result.Model as Cart;

            Assert.IsType<Cart>(actual);
        }
        [Fact]
        public void ConfirmOrder_ShouldReturnExpectedActionName()
        {
            string expected = "ShipmentAddress";
            var action = orderController.ConfirmOrder("send", "");
            var redirect = action as RedirectToActionResult;
            Assert.Equal(expected, redirect.ActionName);
        }
        [Fact]
        public void ShipmentAddressPost_ShouldReturnExpectedActionName()
        {
            string expected = "ConfirmOrder";
            var action = orderController.ShipmentAddress("", "", "");
            var redirect = action as RedirectToActionResult;
            Assert.Equal(expected, redirect.ActionName);
        }
    }
}
