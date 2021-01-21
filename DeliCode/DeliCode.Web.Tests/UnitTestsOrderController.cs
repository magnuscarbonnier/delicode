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
            var orderService = new OrderService(new MockOrderRepository());
            orderController = new OrderController(orderService);
        }

        [Fact]
        public void Index_ShouldReturnViewResultWithoutModel()
        {
            var action = orderController.Index();
            ViewResult result = action as ViewResult;
            var actual = result.Model;

            Assert.Null(actual);
        }
    }
}
