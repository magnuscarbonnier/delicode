﻿using DeliCode.Library.Models;
using DeliCode.Web.Models;
using DeliCode.Web.Repository;
using DeliCode.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliCode.Web.Tests
{
    public class UnitTestsOrderService
    {
        private readonly IOrderService _orderService;
        private readonly MockOrderRepository _repository;
        private readonly Order _order;
        public UnitTestsOrderService()
        {
            _repository = new MockOrderRepository();
            _orderService = new OrderService(_repository);
            _order = new Order
            {
                FirstName = "TestName",
                LastName = "Testsson",
                OrderProducts = new List<OrderProduct>
                {
                    new OrderProduct{
                        Name="TestProduct",
                        Price=29,
                        Quantity=2
                    }
                }
            };
        }
        [Fact]
        public async Task PlaceOrder_ReturnsOrderWithId()
        {
            var order = await _orderService.PlaceOrder(_order);

            Assert.NotEqual(0,order.Id);
        }

        [Fact]
        public async Task PlaceOrder_ThenGetOrder_ReturnsSameOrder()
        {
            var expected = await _orderService.PlaceOrder(_order);
            var actual = await _orderService.GetOrderById(expected.Id);

            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public async Task UpdateOrder_UpdatesOrder()
        {
            var orderId = 1;
            var order = await _orderService.GetOrderById(orderId);
            order.Status = OrderStatus.Delivered;

            Order result = await _orderService.UpdateOrder(orderId, order);

            Assert.Equal(orderId, result.Id);
            Assert.Equal(order.Status, result.Status);
        }
        [Fact]
        public async Task UpdateOrder_WrongId_ReturnsNull()
        {
            var wrongOrderId = 54;
            var orderId = 1;
            var order = await _orderService.GetOrderById(orderId);
            order.Status = OrderStatus.Delivered;

            var result = await _orderService.UpdateOrder(wrongOrderId, order);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllOrders_ReturnsAllOrders()
        {
            var orders = await _orderService.GetOrders();

            Assert.IsType<List<Order>>(orders);
        }
        [Fact]
        public async Task GetAllOrders_ReturnsNullIfEmpty()
        {
            _repository.orders.Clear();
            var orders = await _orderService.GetOrders();

            Assert.Null(orders);
        }

        [Fact]
        public async Task GetOrdersByUserId_ReturnsOrders()
        {
            var userId = _repository.orders.FirstOrDefault().UserId;
            List<Order> orders = await _orderService.GetOrdersByUserId(userId);

            Assert.Collection(orders, order => Assert.Contains(userId, order.UserId));
        }
        [Fact]
        public async Task GetOrdersByInvalidUserId_ReturnsNull()
        {
            string userId = string.Empty;
            var orders = await _orderService.GetOrdersByUserId(userId);

            Assert.Null(orders);
        }

        [Fact]
        public async Task GetOrderById_EmptyDb_ReturnsNull()
        {
            _repository.orders.Clear();

            var orders = await _orderService.GetOrderById(1);

            Assert.Null(orders);
        }

        [Fact]
        public async Task GetOrderById_ReturnsOrders()
        {
            var orderid = 1;

            var order = await _orderService.GetOrderById(orderid);

            Assert.Equal(orderid, order.Id);
        }

        [Fact]
        public async Task DeleteOrderReturnsDeletedOrder()
        {
            var orderId = _repository.orders.FirstOrDefault().Id;
            
            var expected = _repository.GetOrderById(orderId);
            Order actual = await _orderService.DeleteOrder(orderId);

            Assert.Equal(expected.Id, actual.Id);
            var deletedOrder = await _orderService.GetOrderById(orderId);
            Assert.Null(deletedOrder);
        }

        [Fact]
        public async Task DeleteOrderThatDoesntExistReturnsNull()
        {
            var orderID = 355;

            var actual = await _orderService.DeleteOrder(orderID);

            Assert.Null(actual);
        }
    }
}
