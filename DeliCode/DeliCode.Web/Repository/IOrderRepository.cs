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
        Task<Order> GetOrderById(int? id);
        Task<Order> UpdateOrder(Order order);
    }
}
