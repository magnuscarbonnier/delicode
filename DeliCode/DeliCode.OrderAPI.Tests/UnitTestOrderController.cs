using DeliCode.Library.Models;
using DeliCode.OrderAPI.Controllers;
using DeliCode.OrderAPI.Data;
using DeliCode.OrderAPI.Models;
using DeliCode.OrderAPI.Repository;
using DeliCode.OrderAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class UnitTestOrderController
    {
        private readonly Order _order;
        private readonly List<Order> _orderList;
        private readonly OrderRepository _repository;
        private readonly OrdersController _orderController;
        protected DbContextOptions<OrderDbContext> ContextOptions { get; }

        public UnitTestOrderController()
        {
            _order = new Order()
            {
                OrderDate = new DateTime(2020, 11, 20),
                Status = OrderStatus.Delivered,
                UserId = "11223344-5566-7788-99AA-BBCCDDEEFF00",
                Email = "marie.dahlmalm@iths.se",
                FirstName = "Marie",
                LastName = "Dahlmalm",
                Address = "�rsta�ngsv�gen 9",
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

            ContextOptions = SetMockDatabaseOptions();
            SeedMockData();

            _repository = new OrderRepository(new OrderDbContext(ContextOptions));
            _orderController = new OrdersController(_repository);
        }

        private DbContextOptions<OrderDbContext> SetMockDatabaseOptions()
        {
            string connectionString = @"Server=(localdb)\mssqllocaldb;Database=OrderApiControllerTestDB;ConnectRetryCount=0";
            return new DbContextOptionsBuilder<OrderDbContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        [Fact]
        public async Task AddOrder_ShouldReturnNewOrderId()
        {
            var result = await _orderController.AddOrder(_order) as CreatedAtActionResult;
            var order = result.Value as Order;

            Assert.NotEqual(0, order.Id);
        }

        [Fact]
        public async Task AddIncompleteOrder_ShouldReturnBadRequest()
        {
            _order.OrderProducts = null;

            var result = await _orderController.AddOrder(_order);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetOrderById_ShouldReturnSingleOrder()
        {
            int id = await _repository.GetFirstOrderId();

            var result = await _orderController.GetOrderByOrderId(id);
            var okobjresult = result as OkObjectResult;
            var order = okobjresult.Value as Order;

            Assert.IsType<OkObjectResult>(okobjresult);
            Assert.IsType<Order>(order);
            Assert.Equal(id, order.Id);
        }

        [Fact]
        public async Task GetOrderByInvalidId_ShouldReturnNotFound()
        {
            int invalidId = 3;

            var result = await _orderController.GetOrderByOrderId(invalidId);
            var notfoundresult = result as NotFoundResult;

            Assert.IsType<NotFoundResult>(notfoundresult);
        }

        [Fact]
        public async Task GetAllOrdersByUserId_ShouldReturnListOfOrders()
        {
            string userId = _order.UserId;

            var result = await _orderController.GetOrdersByUserId(userId);
            var okobjresult = result as OkObjectResult;
            var orders = okobjresult.Value as List<Order>;

            Assert.IsType<OkObjectResult>(okobjresult);
            Assert.Collection(orders, order => Assert.Contains(userId, order.UserId));
        }

        [Fact]
        public async Task GetAllOrdersByInvalidUserId_ShouldReturnNotFound()
        {
            string userId = String.Empty;

            var result = await _orderController.GetOrdersByUserId(userId);
            var notfoundresult = result as NotFoundResult;

            Assert.IsType<NotFoundResult>(notfoundresult);
        }

        [Fact]
        public async Task GetAllOrders_ShouldReturnListOfOrders()
        {
            int ordersInDb = await _repository.GetOrdersCount();

            var result = await _orderController.GetOrders();
            var okobjresult = result as OkObjectResult;
            var orders = okobjresult.Value as List<Order>;

            Assert.IsType<OkObjectResult>(okobjresult);
            Assert.IsType<List<Order>>(orders);
            Assert.Equal(ordersInDb, orders.Count());
        }

        [Fact]
        public async Task GetAllOrders_NoOrdersAdded_ShouldReturnNoContent()
        {
            var orders = await _repository.GetAllOrders();
            foreach (var order in orders)
            {
                await _repository.DeleteOrder(order.Id);
            }

            var result = await _orderController.GetOrders();
            var noContentResult = result as NoContentResult;

            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact]
        public async Task UpdateOrder_ReturnsUpdatedOrder()
        {
            _order.Id = 1;
            _order.Status = OrderStatus.Refunded;

            var result = await _orderController.UpdateOrder(_order.Id, _order);
            var okobjresult = result as OkObjectResult;
            var updatedOrder = okobjresult.Value as Order;

            Assert.IsType<OkObjectResult>(okobjresult);
            Assert.Equal(OrderStatus.Refunded, updatedOrder.Status);
        }

        [Fact]
        public async Task UpdateOrder_InvalidId_ReturnsBadRequest()
        {
            _order.Status = OrderStatus.Refunded;
            var orders = await _repository.GetAllOrders();
            _order.Id = orders[0].Id;

            var result = await _orderController.UpdateOrder(6, _order);
            var badrequestresult = result as BadRequestResult;

            Assert.IsType<BadRequestResult>(badrequestresult);
        }

        [Fact]
        public async Task UpdateOrder_OrderNotFound_ReturnsBadRequest()
        {
            _order.Status = OrderStatus.Refunded;
            _order.Id = 6;

            var result = await _orderController.UpdateOrder(_order.Id, _order);
            var badrequestresult = result as BadRequestResult;

            Assert.IsType<BadRequestResult>(badrequestresult);
        }

        [Fact]
        public async Task DeleteOrderReturnsDeletedOrder()
        {
            var orders = await _repository.GetAllOrders();
            var orderId = orders.FirstOrDefault().Id;

            var result = await _orderController.DeleteOrder(orderId);
            var okObjectResult = result as OkObjectResult;
            var actual = okObjectResult.Value as Order;

            Assert.IsType<OkObjectResult>(okObjectResult);
            Assert.Equal(orderId, actual.Id);
        }

        [Fact]
        public async Task DeleteOrdeThatDOesntExistReturnsReturnsBadRequest()
        {
            var orderId = 668;

            var result = await _orderController.DeleteOrder(orderId);
            var actual = result as BadRequestResult;

            Assert.IsType<BadRequestResult>(actual);
        }
        private void SeedMockData()
        {
            using (var context = new OrderDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();

                var order1 = new Order()
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
                        Name = "Kladdkaka",
                        Quantity = 11,
                        Price = 11.99M,
                        OrderId = 2000
                    },
                     new OrderProduct()
                    {
                        Name = "Cheesecake",
                        Quantity = 2,
                        Price = 29M,
                        OrderId = 2000
                     }
                }
                };
                var order2 = new Order
                {
                    UserId = "d514be83-bebb-4fe7-b905-e8db158a9ffd"
                };

                context.AddRange(order1, order2);

                context.SaveChanges();
            }
        }
    }
}
