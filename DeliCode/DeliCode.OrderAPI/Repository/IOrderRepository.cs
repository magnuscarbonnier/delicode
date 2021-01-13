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
        //klar
        Task<Order> AddOrder(Order order);
        //klar
        Task<Order> GetOrderById(int id);
        //klar
        Task<List<Order>> GetAllOrdersByUserId(string userId);
        //klar
        Task<List<Order>> GetAllOrders();

        Task<Order> UpdateOrder(Order order);
        Task<Order> DeleteOrder(int orderId);
    }
}   
