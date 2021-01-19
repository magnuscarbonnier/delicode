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

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
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

        //TODO ERik!
        //private async Task<bool> UpdateProductInventory(List<OrderProduct> orderProducts)
        //{
        //    Dictionary<Guid, int> orderProductsQuantity = MapOrderProductsToDictionary(orderProducts);

        //    bool isSuccess = await _productService.UpdateInventoryAmount(orderProductsQuantity);
            
        //    return isSuccess;
        //}
        //TODO Unused
        private Dictionary<Guid, int> MapOrderProductsToDictionary(List<OrderProduct> orderProducts)
        {
            Dictionary<Guid, int> productQuantityValuePairs = new Dictionary<Guid, int>();
            foreach (var product in orderProducts)
            {
                productQuantityValuePairs.Add(product.Id, product.Quantity);
            }

            return productQuantityValuePairs;
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