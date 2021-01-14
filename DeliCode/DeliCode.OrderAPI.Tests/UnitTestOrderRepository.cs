using DeliCode.Library.Models;
using DeliCode.OrderAPI.Data;
using DeliCode.OrderAPI.Models;
using DeliCode.OrderAPI.Repository;
using DeliCode.OrderAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliCode.OrderAPI.Tests
{
    public class UnitTestOrderRepository
    {
        private readonly IOrderRepository _repository;
        protected DbContextOptions<OrderDbContext> ContextOptions { get; }
        private Order _order;
        private int orderProductsInOrder1 = 2;
        public UnitTestOrderRepository()
        {
            ContextOptions = SetMockDatabaseOptions();
            SeedMockData();
            _repository = new OrderRepository(new OrderDbContext(ContextOptions));

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
        }

        [Fact]
        public async Task AddOrder_WithId_ReturnsNull()
        {
            _order.Id = 5;
            var result = await _repository.AddOrder(_order);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddOrder_WithoutId_ReturnsOrderWithId()
        {
            var result = await _repository.AddOrder(_order);

            Assert.IsType<Order>(result);
            Assert.NotEqual(0, result.Id);
            Assert.Equal(orderProductsInOrder1, result.OrderProducts.Count);
        }

        [Fact]
        public async Task AddOrder_WithoutId_ReturnsOrderWithIdAndDateCreated()
        {
            _order.OrderDate = DateTime.MinValue;
            var result = await _repository.AddOrder(_order);

            Assert.IsType<Order>(result);
            Assert.NotEqual(DateTime.MinValue, result.OrderDate);
        }

        [Fact]
        public async Task DeleteOrder_IdNotExists_ReturnsNull()
        {
            var orderidNotInDb = 3;

            var result = await _repository.DeleteOrder(orderidNotInDb);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteOrder_IdExists_ReturnsDeletedOrder()
        {
            var orderidInDb = await _repository.GetFirstOrderId();

            var result = await _repository.DeleteOrder(orderidInDb);
            var tryGetDeletedOrder = await _repository.GetOrderById(orderidInDb);

            Assert.IsType<Order>(result);
            Assert.Equal(orderidInDb, result.Id);
            Assert.Null(tryGetDeletedOrder);
        }

        [Fact]
        public async Task GetOrderById_IdExists_ReturnsOrder()
        {
            var orderIdInDb = await _repository.GetFirstOrderId();

            var result = await _repository.GetOrderById(orderIdInDb);

            Assert.IsType<Order>(result);
            Assert.Equal(orderIdInDb, result.Id);
            Assert.Equal(orderProductsInOrder1, result.OrderProducts.Count);
        }

        [Fact]
        public async Task GetOrderById_IdNotExists_ReturnsNull()
        {
            var orderIdNotInDb = 3;

            var result = await _repository.GetOrderById(orderIdNotInDb);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetOrderByUserId_IdExists_ReturnsOrder()
        {
            var expectedorderid = 1;
            var expectedcount = 1;

            var result = await _repository.GetAllOrdersByUserId(_order.UserId);

            Assert.IsType<List<Order>>(result);
            Assert.Equal(expectedcount, result.Count());
            Assert.Equal(expectedorderid, result.FirstOrDefault().Id);
            Assert.Equal(orderProductsInOrder1, result.FirstOrDefault().OrderProducts.Count);
        }

        [Fact]
        public async Task GetOrderByUserId_IdNotExists_ReturnsEmtyList()
        {
            var userIdNotInDb = String.Empty;

            var result = await _repository.GetAllOrdersByUserId(userIdNotInDb);

            Assert.IsType<List<Order>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllOrders_ReturnsListOfOrder()
        {
            var result = await _repository.GetAllOrders();

            Assert.IsType<List<Order>>(result);
            Assert.Equal(orderProductsInOrder1, result.FirstOrDefault().OrderProducts.Count);
        }

        [Fact]
        public async Task GetAllOrders_EmptyDb_ReturnsEmtyList()
        {
            foreach (var order in await _repository.GetAllOrders())
            {
                await _repository.DeleteOrder(order.Id);
            }

            var result = await _repository.GetAllOrders();

            Assert.IsType<List<Order>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetFirstOrderId_ReturnsOrderId()
        {
            var result = await _repository.GetFirstOrderId();

            Assert.IsType<int>(result);
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task GetFirstOrderId_EmptyDatabase_ReturnsDefaultInt()
        {
            foreach (var order in await _repository.GetAllOrders())
            {
                await _repository.DeleteOrder(order.Id);
            }

            var result = await _repository.GetFirstOrderId();

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task UpdateOrder_OrderNotExists_ReturnNull()
        {
            var orderidnotindb = 4;
            var order = new Order { Id = orderidnotindb, FirstName = "Test" };

            var result = await _repository.UpdateOrder(order);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateOrder_OrderExists_ReturnUpdatedOrder()
        {
            var address = "Storgatan";
            var order = await _repository.GetFirstOrder();
            order.Address = address;

            var result = await _repository.UpdateOrder(order);

            Assert.Equal(address,result.Address);
            Assert.IsType<Order>(result);
            Assert.Equal(orderProductsInOrder1, result.OrderProducts.Count);
        }

        [Fact]
        public async Task GetFirstOrder_ReturnsOrder()
        {
            var result = await _repository.GetFirstOrder();

            Assert.IsType<Order>(result);
            Assert.NotEqual(0, result.Id);
            Assert.Equal(orderProductsInOrder1, result.OrderProducts.Count);
        }

        [Fact]
        public async Task GetFirstOrder_EmptyDatabase_ReturnsNull()
        {
            foreach (var order in await _repository.GetAllOrders())
            {
                await _repository.DeleteOrder(order.Id);
            }

            var result = await _repository.GetFirstOrder();

            Assert.Null(result);
        }

        [Fact]
        public async Task GetOrdersCount_ReturnsOrdersCount()
        {
            int ordersinDb = 2;

            int result = await _repository.GetOrdersCount();

            Assert.Equal(ordersinDb, result);
        }

        [Fact]
        public async Task GetOrdersCount_EmptyDb_ReturnsOrdersCount()
        {
            foreach (var order in await _repository.GetAllOrders())
            {
                await _repository.DeleteOrder(order.Id);
            }
            int ordersinDb = 0;

            int result = await _repository.GetOrdersCount();

            Assert.Equal(ordersinDb, result);
        }

        private DbContextOptions<OrderDbContext> SetMockDatabaseOptions()
        {
            string connectionString = @"Server=(localdb)\mssqllocaldb;Database=OrderApiRepositoryTestDB;ConnectRetryCount=0";
            return new DbContextOptionsBuilder<OrderDbContext>()
                .UseSqlServer(connectionString)
                .Options;
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
