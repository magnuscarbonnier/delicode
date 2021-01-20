using DeliCode.Web.Models;
using DeliCode.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliCode.Web.Tests
{
    public class UnitTestsInventoryService
    {
        private readonly InventoryService _service;
        private List<OrderProduct> orderProducts;
        private Order order;
        public UnitTestsInventoryService()
        {
            MockProductService mockProductService = new MockProductService();
            MockOrderService mockOrderService = new MockOrderService();
            _service = new InventoryService(mockProductService, mockOrderService);

            orderProducts = new List<OrderProduct>()
            {
                new OrderProduct()
                {
                     ProductId = Guid.Parse("e6d22bf9-6ab7-4ebf-811d-17ef1d227468"),

                }
            };
            order = new Order() { OrderProducts = orderProducts };
        }


        [Fact]
        public async Task ValidateInventoryReturnsTrueIfQuantitiesAreOK()
        {
            var orderProduct = orderProducts.First();
            orderProduct.Quantity = 2;

            var actual = await _service.ValidateInventory(orderProducts);

            Assert.True(actual);
        }

        [Fact]
        public async Task ValidateInventoryReturnsFalseIfQuantitiesAreTooLow()
        {
            var orderProduct = orderProducts.First();
            orderProduct.Quantity = 10;

            var actual = await _service.ValidateInventory(orderProducts);

            Assert.False(actual);
        }

        [Fact]
        public async Task FinalizeOrderReturnsNullWhenValidateInventoryFails()
        {
            var orderProduct = orderProducts.First();
            orderProduct.Quantity = 10;

            var actual = await _service.FinalizeOrder(order);

            Assert.Null(actual);
        }

        [Fact]
        public async Task FinalizeOrderReturnsNullWhenPlaceOrderFails()
        {
            order.UserId = null;

            var actual = await _service.FinalizeOrder(order);

            Assert.Null(actual);
        }
        [Fact]
        public async Task FinalizeOrder_DeletesOrderAndMarksAsRefunded_If_InventoryUpdateFails()
        {
            order.Id = 1;
            order.UserId = "2";

            var actual = await _service.FinalizeOrder(order);

            Assert.Equal(Library.Models.OrderStatus.Refunded, actual.Status);
        }
    }

    internal class MockOrderService : IOrderService
    {
        public Task<Order> DeleteOrder(int orderId)
        {
            Order order = new Order() { Id = orderId, Status = Library.Models.OrderStatus.Refunded };

            return Task.FromResult(order);
        }

        public Task<Order> GetOrderById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetOrders()
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetOrdersByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Order> PlaceOrder(Order order)
        {
            if (order.UserId == null)
            {
                order = null;
            }

            return Task.FromResult(order);
        }

        public Task<Order> UpdateOrder(int orderId, Order order)
        {
            throw new NotImplementedException();
        }
    }

    internal class MockProductService : IProductService
    {
        private List<Product> products;

        public MockProductService()
        {
            products = new List<Product>()
            {
                 new Product()
                 {
                      AmountInStorage = 5,
                       Id = Guid.Parse("e6d22bf9-6ab7-4ebf-811d-17ef1d227468")
                 }
            };
        }

        public Task<Product> Add(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<Product> Get(Guid id)
        {
            return Task.FromResult(products.FirstOrDefault(p => p.Id == id));
        }

        public Task<List<Product>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Product> Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> Update(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateInventoryAmount(List<OrderProduct> orderProducts)
        {
            bool updateSuccess = false;

            return Task.FromResult(updateSuccess);
        }
    }
}
