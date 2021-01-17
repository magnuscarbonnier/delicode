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
        private readonly IProductService _productService;
        public OrderService(IOrderRepository repository, IProductService productService)
        {
            _repository = repository;
            _productService = productService;
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
            var newOrder = new Order();
            bool isSuccess = await CheckProductAmount(order.OrderProducts);
            if (isSuccess)
            {
                newOrder = await _repository.PlaceOrder(order);
            }
            else
            {
                newOrder = null;
            }
            return newOrder;
        }

        private async Task<bool> CheckProductAmount(List<OrderProduct> orderProducts)
        {
            var isSuccess = false;
            foreach (var orderProduct in orderProducts)
            {
                //TODO: not finished
                var blah=new KeyValuePair<Guid, int>(orderProduct.ProductId, orderProduct.Quantity);

                //var product = await _productService.Get(orderProduct.ProductId);

                //product.AmountInStorage = product.AmountInStorage - orderProduct.Quantity;
               bool x = await _productService.UpdateAmount(new KeyValuePair<Guid, int>(orderProduct.ProductId,orderProduct.Quantity));
            }
            return isSuccess;
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