using DeliCode.Web.Models;
using DeliCode.Web.Repository;
using DeliCode.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IProductService _productService; //TODO Tight coupling needs to be revised.

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
            _productService = new ProductService(new System.Net.Http.HttpClient());
        }

        public async Task<Order> DeleteOrder(int orderId)
        {
            Order order = await _repository.DeleteOrder(orderId);
            return order;
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await _repository.GetOrderById(id);
            return order;
        }

        public async Task<List<Order>> GetOrders()
        {
            var orders = await _repository.GetAll();
            if (orders == null || !orders.Any())
            {
                return null;
            }
            return orders;

        }

        public async Task<List<Order>> GetOrdersByUserId(string userId)
        {
            List<Order> orders = await _repository.GetOrdersByUsersId(userId);
            if (!orders.Any())
            {
                return null;
            }
            return orders;
        }

        public async Task<Order> PlaceOrder(Order order)
        {
            var newOrder = await _repository.PlaceOrder(order);

            return newOrder;
        }

        public async Task<Order> UpdateOrder(int orderId, Order order)
        {
            if (orderId != order.Id)
            {
                return null;
            }
            order = await _repository.UpdateOrder(order);
            return order;
        }
    }
}