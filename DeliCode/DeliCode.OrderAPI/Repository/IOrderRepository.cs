using DeliCode.Library.Models;
using DeliCode.OrderAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.OrderAPI.Services
{
    public interface IOrderRepository
    {
        Task<Order> AddOrder(Order order);
        Task<Order> GetOrderById(Guid id);
        List<Order> GetAllOrdersByUserId(string userId);
        List<Order> DeleteOrderByOrderId(Guid id);
        Task<ActionResult<IEnumerable<Order>>> GetAllOrders();
        void UpdateOrder(Order order);
    }
}   
