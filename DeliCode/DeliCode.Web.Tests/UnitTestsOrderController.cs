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
        private string empty = String.Empty;
        public UnitTestsOrderController()
        {
            orderController = new OrderController();
        }

        [Fact]
        public void Index_ShouldReturnViewResultWithoutModel()
        {
            OrderSummary.Cart = new Cart();
            var action = orderController.Index();
            ViewResult result = action as ViewResult;
            var actual = result.Model;

            Assert.Null(actual);
        }
        [Fact]
        public void ConfirmOrder_WithoutStringSend_ShouldReturnViewResultWithModelCart()
        {
            OrderSummary.Cart = new Cart();

            var action = orderController.ConfirmOrder(empty, empty);
            var viewResult = action as ViewResult;
            var actualCart = viewResult.Model as Cart;

            Assert.IsType<Cart>(actualCart);
        }
        [Fact]
        public void ConfirmOrder_ShouldReturnExpectedActionName()
        {
            string expectedAction = "ShipmentAddress";
            string expectedController = "Order";

            var action = orderController.ConfirmOrder("send", empty);
            var redirectAction = action as RedirectToActionResult;

            Assert.Equal(expectedAction, redirectAction.ActionName);
            Assert.Equal(expectedController, redirectAction.ControllerName);
        }
        [Fact]
        public void ShipmentAddressPost_ShouldReturnExpectedActionName()
        {
            string expectedAction = "ConfirmOrder";
            string expectedController = "Order";

            var action = orderController.ShipmentAddress(empty, empty, empty);
            var redirect = action as RedirectToActionResult;
            
            Assert.Equal(expectedAction, redirect.ActionName);
            Assert.Equal(expectedController, redirect.ControllerName);
        }
    }
}
