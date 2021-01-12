using DeliCode.Library.Models;
using DeliCode.Web.Models;
using DeliCode.Web.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliCode.Web.Tests
{
    public class MockOrderRepository : IOrderRepository
    {
        private List<Order> orders;
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
        public Task<Order> GetOrderById(int? id)
        {
            var order = orders.SingleOrDefault(o => o.Id == id);
            return Task.FromResult(order);
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
