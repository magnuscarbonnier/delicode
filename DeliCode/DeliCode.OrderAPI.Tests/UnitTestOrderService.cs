using DeliCode.Library.Models;
using DeliCode.OrderAPI.Models;
using DeliCode.OrderAPI.Repository;
using DeliCode.OrderAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DeliCode.OrderAPI.Tests
{
    public class UnitTestOrderService
    {
        private readonly Order _order;
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly OrderService _service;
        private List<Order> _orders;

        public UnitTestOrderService()
        {
            _order = new Order()
            {
                Id = new Guid("fb6f6dd2-f6c5-4893-ab35-03167f6ebe28"),
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
                        Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                        Name = "Kladdkaka",
                        Quantity = 11,
                        Price = 11.99M,
                        OrderId = new Guid("47ffb3b4-4e4e-40a9-88c7-09c995f1ec0b")
                    },
                     new OrderProduct()
                    {
                        Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF11"),
                        Name = "Cheesecake",
                        Quantity = 2,
                        Price = 29M,
                        OrderId = new Guid("5cea861a-dfe8-4a5e-9f1b-6d90ae655401")
                     }
                }
            };
            _orderServiceMock = new Mock<IOrderService>();
            _service = new OrderService(
            _orderServiceMock.Object);
            _orders = new List<Order> { _order };
        }

        [Fact]
        public void AddOrder_ShouldAddOrderToList()
        {
            Order placedOrder = null;
            _orderServiceMock.Setup(o => o.AddOrder(It.IsAny<Order>()))
                .Callback<Order>(orderObject =>
                {
                    placedOrder = orderObject;
                });

            _service.AddOrder(_order);

            _orderServiceMock.Verify(x => x.AddOrder(It.IsAny<Order>()), Times.Once);

            Assert.NotNull(placedOrder);
            Assert.Equal(_order.Id, placedOrder.Id);
            Assert.Equal(_order.FirstName, placedOrder.FirstName);
            Assert.Equal(_order.LastName, placedOrder.LastName);
            Assert.Equal(_order.Phone, placedOrder.Phone);
            Assert.Equal(_order.Address, placedOrder.Address);
            Assert.Equal(_order.City, placedOrder.City);
            Assert.Equal(_order.Country, placedOrder.Country);
            Assert.Equal(_order.ZipCode, placedOrder.ZipCode);
            Assert.Equal(_order.Email, placedOrder.Email);
            Assert.Equal(_order.ShippingNotes, placedOrder.ShippingNotes);
            Assert.Equal(_order.Status, placedOrder.Status);
            Assert.Equal(_order.OrderDate, placedOrder.OrderDate);
            Assert.Equal(_order.UserId, placedOrder.UserId);
            Assert.Equal(_order.OrderProducts, placedOrder.OrderProducts);
        }

        [Fact]
        public void GetOrderById_ShouldReturnSingleOrder()
        {
            var expected = _service.GetOrderById(new Guid("fb6f6dd2-f6c5-4893-ab35-03167f6ebe28"));
            var actual = _service.orders[0];
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAllOrdersByUserId_ShouldReturnListOfOrders()
        {
            var expected = _service.orders.Where(x => x.UserId == "11223344-5566-7788-99AA-BBCCDDEEFF00");
            List<Order> actual = _service.GetAllOrdersByUserId("11223344-5566-7788-99AA-BBCCDDEEFF00");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeleteOrderByOrderId_ShouldDeleteOrderFromList()
        {
            var expected = _service.orders.Count() - 1;
            int actual = _service.DeleteOrderByOrderId(new Guid("fb6f6dd2-f6c5-4893-ab35-03167f6ebe28")).Count();
            Assert.Equal(expected, actual);
        }
    }
}
