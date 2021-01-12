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
            _context.Orders.Add(order);
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

        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders.Include(o => o.OrderProducts).ToListAsync();
        }

        public async Task<List<Order>> GetAllOrdersByUserId(string userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId).Include(op => op.OrderProducts).ToListAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders.Include(op => op.OrderProducts).SingleOrDefaultAsync(o => o.Id == id);
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
