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
    }
}
