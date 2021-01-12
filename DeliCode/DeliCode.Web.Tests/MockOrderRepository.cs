using DeliCode.Library.Models;
using DeliCode.Web.Models;
using DeliCode.Web.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DeliCode.Web.Tests")]

namespace DeliCode.Web.Tests
{
    public class MockOrderRepository : IOrderRepository
    {
        internal List<Order> orders;
        public MockOrderRepository()
        {
            orders = new List<Order>
            {
                new Order
                {
                    Id=1,
                    FirstName = "TestName",
                    LastName = "Testsson",
                    Status=OrderStatus.Recieved,
                    UserId = "22223344-5566-7788-99AA-BBCCDDEEFF11",
                    OrderProducts = new List<OrderProduct>
                 
                    {
                        new OrderProduct{
                        Name="TestProduct",
                        Price=29,
                        Quantity=2
                    }
                }

                }
            };

        }

        public Task<List<Order>> GetAll()
        {
            return Task.FromResult(orders);
        }

        public Task<Order> GetOrderById(int? id)
        {
            var order = orders.SingleOrDefault(o => o.Id == id);
            return Task.FromResult(order);
        }

        public Task<List<Order>> GetOrdersByUsersId(string userId)
        {
            var orderToReturn = orders.Where(o => o.UserId == userId).ToList();
            return Task.FromResult(orderToReturn);
        }

        public Task<Order> PlaceOrder(Order order)
        {
            order.Id = 3;
            orders.Add(order);
            return Task.FromResult(order);
        }

        public Task<Order> UpdateOrder(Order order)
        {
            var orderToUpdate = orders.SingleOrDefault(o => o.Id == order.Id);
            orderToUpdate = order;
            return Task.FromResult(orderToUpdate);
        }
    }
}
