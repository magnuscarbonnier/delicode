using DeliCode.Library.Models;
using DeliCode.OrderAPI.Data;
using DeliCode.OrderAPI.Models;
using DeliCode.OrderAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DeliCode.OrderAPI.Tests")]
namespace DeliCode.OrderAPI.Repository
{
    public class MockOrderRepository : IOrderRepository
    {
        protected DbContextOptions<OrderDbContext> ContextOptions { get; }
        private readonly OrderDbContext _context;
        public MockOrderRepository()
        {
            ContextOptions = new DbContextOptionsBuilder<OrderDbContext>()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFTestSample;ConnectRetryCount=0")
                .Options;

            Seed();
            _context = new OrderDbContext(ContextOptions);
        }

        private void Seed()
        {
            using (var context = new OrderDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();

                var order1 = new Order()
                {
                    
                    OrderDate = new DateTime(2020, 11, 20),
                    Status = OrderStatus.Delivered,
                    UserId = "11223344-5566-7788-99AA-BBCCDDEEFF00",
                    Email = "marie.dahlmalm@iths.se",
                    FirstName = "Marie",
                    LastName = "Dahlmalm",
                    Address = "Årstaängsvägen 9",
                    ZipCode = "12345",
                    City = "Stockholm",
                    Country = "Sweden",
                    Phone = "555123456",
                    ShippingNotes = "",
                    OrderProducts = new List<OrderProduct>()
                {
                     new OrderProduct()
                    {
                        
                        Name = "Kladdkaka",
                        Quantity = 11,
                        Price = 11.99M,
                        OrderId = 2000
                    },
                     new OrderProduct()
                    {
                        
                        Name = "Cheesecake",
                        Quantity = 2,
                        Price = 29M,
                        OrderId = 2000
                     }
                }
                };
                var order2 = new Order
                {
                   
                    UserId = "d514be83-bebb-4fe7-b905-e8db158a9ffd"
                };


                context.AddRange(order1, order2);

                context.SaveChanges();
            }
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
