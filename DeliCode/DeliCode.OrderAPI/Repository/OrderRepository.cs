using DeliCode.OrderAPI.Data;
using DeliCode.OrderAPI.Models;
using DeliCode.OrderAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository

    {
        private readonly OrderDbContext _context;
        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }
        public async Task<Order> AddOrder(Order order)
        {
            try
            {
                order.OrderDate = DateTime.UtcNow;
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }
            catch
            {
                order = null;
            }

            return order;
        }

        public async Task<Order> DeleteOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            try
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            catch
            {
                order = null;
            }

            return order;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders.Include(o => o.OrderProducts).ToListAsync();
        }

        public async Task<List<Order>> GetAllOrdersByUserId(string userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId).Include(op => op.OrderProducts).ToListAsync();
        }

        public async Task<Order> GetFirstOrder()
        {
            var order = await _context.Orders.Include(op => op.OrderProducts).FirstOrDefaultAsync();

            return order;
        }


        public async Task<int> GetFirstOrderId()
        {
            var order = await _context.Orders.FirstOrDefaultAsync();
            if (order == null)
            {
                return default;
            }
            return order.Id;
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders.Include(op => op.OrderProducts).SingleOrDefaultAsync(o => o.Id == id);
        }

        public Task<int> GetOrdersCount()
        {
            var ordersCount = _context.Orders.Count();
            return Task.FromResult(ordersCount);
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                order = null;
            }

            return order;
        }
    }
}
