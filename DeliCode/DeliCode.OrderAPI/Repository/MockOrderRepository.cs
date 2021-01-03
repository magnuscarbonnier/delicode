using DeliCode.Library.Models;
using DeliCode.OrderAPI.Models;
using DeliCode.OrderAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DeliCode.OrderAPI.Tests")]
namespace DeliCode.OrderAPI.Repository 
{
    public class MockOrderRepository : IOrderRepository
    {
        internal List<Order> orders = new List<Order>()
        {
            new Order()
           {
                Id = new Guid("fb6f6dd2-f6c5-4893-ab35-03167f6ebe28"),
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
                        Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                        Name = "Kladdkaka",
                        Quantity = 11,
                        Price = 11.99M,
                        OrderId = new Guid("fb6f6dd2-f6c5-4893-ab35-03167f6ebe28")
                    },
                     new OrderProduct()
                    {
                        Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF11"),
                        Name = "Cheesecake",
                        Quantity = 2,
                        Price = 29M,
                        OrderId = new Guid("fb6f6dd2-f6c5-4893-ab35-03167f6ebe28")
                     }
                }
            }
        };

        public Order GetOrderById(Guid id)
        {
            Order order = orders.Where(x => x.Id == id).SingleOrDefault();
            return order;
        }

        public List<Order> AddOrder(Order order)
        {

            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            var orderReturn = CreateOrder(order);
            var returnList = new List<Order>();
            returnList.Add(orderReturn);
            return returnList;
        }
        private static Order CreateOrder(Order order)
        {
            return new Order
            {
                Id = order.Id,
                FirstName = order.FirstName,
                LastName = order.LastName,
                Email = order.Email,
                Address = order.Address,
                ZipCode = order.ZipCode,
                City = order.City,
                Country = order.Country,
                OrderDate = order.OrderDate,
                Phone = order.Phone,
                ShippingNotes = order.ShippingNotes,
                Status = order.Status,
                UserId = order.UserId,
                OrderProducts = order.OrderProducts
            };
        }
        public List<Order> GetAllOrdersByUserId(string userId)
        {
            List<Order> ordersList = orders.Where(x => x.UserId == userId).OrderBy(d => d.OrderDate).ToList();
            return ordersList;
        }
        public List<Order> DeleteOrderByOrderId(Guid id)
        {
            var orderToDelete = orders.Where(x => x.Id == id).SingleOrDefault();
            orders.Remove(orderToDelete);
            return orders;
        }
    }
}
