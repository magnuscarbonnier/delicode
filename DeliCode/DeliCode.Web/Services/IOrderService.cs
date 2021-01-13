using DeliCode.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.Web.Services
{
    public interface IOrderService
    {
        Task<Order> PlaceOrder(Order order);
        Task<Order> GetOrderById(int? id);
        Task<Order> UpdateOrder(int orderId, Order order);
        Task<List<Order>> GetOrders();
        Task<List<Order>> GetOrdersByUserId(string userId);
        Task<Order> DeleteOrder(int? orderId);
    }
}
