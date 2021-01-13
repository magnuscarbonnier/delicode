using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.Web.Controllers
{
    public class AdminController : Controller
    {
        public List<ExampleOrderViewModel> orders = new List<ExampleOrderViewModel>();
        public AdminController()
        {
            var order = new ExampleOrderViewModel
            {
                OrderDate = DateTime.Now,
                FirstName = "Mikael",
                LastName = "Tallbo",
                UserId = "ABC123",
                Email = "tallbo@gmail.com",
                Address = "Lötsjövägen 47",
                ZipCode = "174 43",
                City = "Sundbyberg",
                Country = "Sweden",
                Phone = "073 111 22 33",
                ShippingNotes = "Endast glutenfri tack annars blir jag arg",
                OrderProducts = new List<ExampleOrderProduct>()
            };
            var order2 = new ExampleOrderViewModel
            {
                OrderDate = DateTime.Now,
                FirstName = "Mangemakerz",
                LastName = "CarbonAir",
                UserId = "EFG123",
                Email = "carbon@carbonair.com",
                Address = "Uppsalavägen",
                ZipCode = "111 22",
                City = "Uppsala",
                Country = "Sweden",
                Phone = "073 222 33 44",
                ShippingNotes = "Föredrar om ni fraktar den på en boeing 747",
                OrderProducts = new List<ExampleOrderProduct>()
            };
            var orderProduct = new ExampleOrderProduct
            {
                Name = "En macka",
                Order = order,
                OrderId = order.Id,
                Price = 40,
                Quantity = 2
            };
            var orderProduct2 = new ExampleOrderProduct
            {
                Name = "En tårta",
                Order = order,
                OrderId = order.Id,
                Price = 200,
                Quantity = 1
            };
            order.OrderProducts.Add(orderProduct);
            order2.OrderProducts.Add(orderProduct2);
            order.Id = Guid.NewGuid();
            order2.Id = Guid.NewGuid();
            orders.Add(order);
            orders.Add(order2);
        }
        public IActionResult Index()
        {
            return View(orders);
        }

        [HttpGet]
        public IActionResult EditOrder(ExampleOrderViewModel order)
        {
            return View(order);
        }
    }
    public class ExampleOrderViewModel
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string ShippingNotes { get; set; }
        public decimal TotalPrice()
        {
            return OrderProducts.Sum(x => x.Price * x.Quantity);
        }
        public List<ExampleOrderProduct> OrderProducts { get; set; }
        public enum OrderStatus { Recieved, Delivered, Refunded }

    }
    public class ExampleOrderProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Guid OrderId { get; set; }
        public ExampleOrderViewModel Order { get; set; }
    }
}
