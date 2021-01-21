using DeliCode.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.Web.Repository
{
    public interface IOrderRepository
    {
        Task<Order> PlaceOrder(Order order);
        Task<Order> GetOrderById(int id);
        Task<Order> UpdateOrder(Order order);
        Task<List<Order>> GetAll();
        Task<List<Order>> GetOrdersByUsersId(string userId);
        Task<Order> DeleteOrder(int orderId);
    }
}
