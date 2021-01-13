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
    public class UnitTestsAdminController
    {
        private AdminController _controller;
        private readonly IOrderService _orderService;
        private readonly MockOrderRepository _repository;
        public UnitTestsAdminController()
        {
            _repository = new MockOrderRepository();
            _orderService = new OrderService(_repository);
            _controller = new AdminController(_orderService);
        }

        [Fact]
        public async Task AdminIndexGetOrders_OrderListIsEmpty_ReturnsNewListOfOrders()
        {
            _repository.orders.Clear();

            var result = await _controller.Index();
            var viewresult = result as ViewResult;
            var orders = viewresult.Model as List<Order>;
         
            Assert.Empty(orders);
        }

        [Fact]
        public async Task AdminIndexGetOrders_OrderListNotEmpty_ReturnsListOfOrders()
        {
            var result = await _controller.Index();
            var viewresult = result as ViewResult;
            var orders = viewresult.Model as List<Order>;

            Assert.NotEmpty(orders);
        }
    }
}
