using DeliCode.Library.Models;
using DeliCode.OrderAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.OrderAPI.Services
{
    public interface IOrderService
    {
        List<Order> AddOrder(Order order);
        Order GetOrderById(Guid id);
        List<Order> GetAllOrdersByUserId(string userId);
        List<Order> DeleteOrderByOrderId(Guid id);
    }
}
