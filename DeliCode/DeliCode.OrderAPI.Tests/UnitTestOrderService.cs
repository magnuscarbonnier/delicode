using DeliCode.Library.Models;
using DeliCode.OrderAPI.Controllers;
using DeliCode.OrderAPI.Models;
using DeliCode.OrderAPI.Repository;
using DeliCode.OrderAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DeliCode.OrderAPI.Tests
{
    public class UnitTestOrderService
    {
        private readonly Order _order;
        private readonly List<Order> _orderList;
        private readonly MockOrderRepository _service;
        private readonly OrderController orderController;

        public UnitTestOrderService()
        {
            _order = new Order()
            {
                OrderDate = new DateTime(2020, 11, 20),
                Status = OrderStatus.Delivered,
                UserId = "11223344-5566-7788-99AA-BBCCDDEEFF00",
                Email = "marie.dahlmalm@iths.se",
                FirstName = "Marie",
                LastName = "Dahlmalm",
                Address = "Årstaängsvägen 9",
                ZipCode = "12345",
                City = "Stockholm",
                Country = "Sweden",
                Phone = "555123456",
                ShippingNotes = "",
                OrderProducts = new List<OrderProduct>()
                {
                    new OrderProduct()
                    {
                        ProductId = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                        Name = "Kladdkaka",
                        Quantity = 11,
                        Price = 11.99M,
                    },
                     new OrderProduct()
                    {
                        ProductId = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF11"),
                        Name = "Cheesecake",
                        Quantity = 2,
                        Price = 29M,
                     }
                }
            };
            _orderList = new List<Order> { _order };
            
            _service = new MockOrderRepository();
            orderController = new OrderController(_service);
        }

        [Fact]
        public async Task AddOrder_ShouldReturnNewOrderId()
        {
            var result = await orderController.AddOrder(_order) as CreatedAtActionResult;
            var order = result.Value as Order;

            Assert.NotEqual(Guid.Empty, order.Id);
        }
        [Fact]
        public async Task AddIncompleteOrder_ShouldReturnBadRequest()
        {
            _order.OrderProducts = null;

            var result = await orderController.AddOrder(_order);

            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact]
        public async Task GetOrderById_ShouldReturnSingleOrder()
        {
            Order order;
            Guid id = new Guid("fb6f6dd2-f6c5-4893-ab35-03167f6ebe28");

            var result = await orderController.GetOrderByOrderId(id);
            order = result.Value;

            Assert.IsType<ActionResult<Order>>(result);
            Assert.IsType<Order>(order);
            Assert.Equal(id, order.Id);
        }

        [Fact]
        public async Task GetOrderByInvalidId_ShouldReturnNull()
        {
            Guid id = new Guid("fb6f6dd2-f6c5-4893-ab35-03167f6ebe29");

            var result = await orderController.GetOrderByOrderId(id);

            Assert.Null(result.Value);
        }

        [Fact]
        public async Task GetAllOrdersByUserId_ShouldReturnListOfOrders()
        {
            string userId = "11223344-5566-7788-99AA-BBCCDDEEFF00";

            var result = await orderController.GetOrdersByUserId(userId);
            var okobjresult = result.Result as OkObjectResult;
            var orders = okobjresult.Value as List<Order>;
            
            Assert.IsType<OkObjectResult>(okobjresult);
            Assert.Collection(orders, order => Assert.Contains(userId, order.UserId));
        }

        [Fact]
        public async Task GetAllOrdersByInvalidUserId_ShouldReturnNotFound()
        {
            string userId = String.Empty;

            var result = await orderController.GetOrdersByUserId(userId);
            var notfoundresult = result.Result as NotFoundResult;

            Assert.IsType<NotFoundResult>(notfoundresult);
        }
        [Fact]
        public async Task GetAllOrders_ShouldReturnListOfOrders()
        {
            var order = new Order
            {
                Id = new Guid("ed9ef515-8735-4116-b444-8a42b187bbfa"),
                UserId = "d514be83-bebb-4fe7-b905-e8db158a9ffd"
            };
            _orderList.Add(order);
            
            var result = await orderController.GetOrders();
            var okobjresult = result.Result as OkObjectResult;
            var orders = okobjresult.Value as List<Order>;
            
            Assert.IsType<OkObjectResult>(okobjresult);
            Assert.Equal(_orderList.Count(), orders.Count());
        }
        [Fact]
        public async Task GetAllOrders_NoOrdersAdded_ShouldReturnNoContent()
        {
            _service.orders.Clear();

            var result = await orderController.GetOrders();
            var noContentResult = result.Result as NoContentResult;

            Assert.IsType<NoContentResult>(noContentResult);
        }
        [Fact]
        public async Task UpdateOrder_ReturnsUpdatedOrder()
        {
            _order.Id = _service.orders[0].Id;
            _order.Status = OrderStatus.Refunded;

            var result = await orderController.UpdateOrder(_order.Id, _order);
            var okobjresult = result.Result as OkObjectResult;
            var updatedOrder = okobjresult.Value as Order;

            Assert.IsType<OkObjectResult>(okobjresult);
            Assert.Equal(_order.Status, updatedOrder.Status);
        }

        [Fact]
        public async Task UpdateOrder_InvalidId_ReturnsBadRequest()
        {
            _order.Status = OrderStatus.Refunded;
            _order.Id = _service.orders[0].Id;

            var result = await orderController.UpdateOrder(Guid.NewGuid(), _order);
            var badrequestresult = result.Result as BadRequestResult;

            Assert.IsType<BadRequestResult>(badrequestresult);
        }
        [Fact]
        public async Task UpdateOrder_OrderNotFound_ReturnsBadRequest()
        {
            _order.Status = OrderStatus.Refunded;
            _service.orders[0].Id = Guid.NewGuid();

            var result = await orderController.UpdateOrder(_order.Id, _order);
            var badrequestresult = result.Result as BadRequestResult;

            Assert.IsType<BadRequestResult>(badrequestresult);
        }
    }
}
