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
        private readonly Mock<IOrderRepository> _orderServiceMock;
        private readonly MockOrderRepository _service;
        private readonly OrderController orderController;
        
        private List<Order> _orders;

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
            _orderServiceMock = new Mock<IOrderRepository>();
            _service = new MockOrderRepository();
            _orders = new List<Order> { _order };
            orderController = new OrderController(_service);
        }

        [Fact]
        public async Task AddOrder_ShouldReturnNewOrderId()
        {
            var result = await orderController.AddOrder(_order) as CreatedAtActionResult;
            var order = result.Value as Order;

            Assert.NotEqual(Guid.Empty,order.Id);
        }
        [Fact]
        public async Task AddIncompleteOrder_ShouldReturnBadRequest()
        {
            _order.OrderProducts = null;

            var result = await orderController.AddOrder(_order);

            Assert.IsType<BadRequestResult>(result);
        }


        [Fact]
        public async Task GetOrderById_ShouldReturnSingleOrderAsync()
        {
            Guid id = new Guid("fb6f6dd2-f6c5-4893-ab35-03167f6ebe28");

            var result = await orderController.GetSingleOrderByOrderId(id);

            Assert.IsType<ActionResult<Order>>(result);
            Assert.IsType<Order>(result.Value);
            Assert.Equal(id, result.Value.Id);

        }

        [Fact]
        public async Task GetOrderByInvalidId_ShouldReturnNull()
        {
            Guid id = new Guid("fb6f6dd2-f6c5-4893-ab35-03167f6ebe29");

            var result = await orderController.GetSingleOrderByOrderId(id);

            Assert.Null(result.Value);
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
