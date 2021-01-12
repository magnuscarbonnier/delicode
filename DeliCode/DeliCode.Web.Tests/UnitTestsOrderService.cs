using DeliCode.Library.Models;
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
        private readonly IOrderRepository _repository;
        private Order _order;
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

            Assert.NotNull(order.Id);
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
    }
}
