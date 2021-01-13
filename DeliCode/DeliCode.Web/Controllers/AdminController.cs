using DeliCode.Web.Models;
using DeliCode.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IOrderService _orderService;
        private List<Order> orders=new List<Order>();
        public AdminController(IOrderService orderService)
        {
             var order = new Order
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
                OrderProducts = new List<OrderProduct>()
            };

            var order2 = new Order
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
                OrderProducts = new List<OrderProduct>()
            };

            var orderProduct = new OrderProduct
            {
                Name = "En macka",
                Order = order,
                OrderId = order.Id,
                Price = 40,
                Quantity = 2
            };

            var orderProduct2 = new OrderProduct
            {
                Name = "En tårta",
                Order = order,
                OrderId = order.Id,
                Price = 200,
                Quantity = 1
            };

            order.OrderProducts.Add(orderProduct);
            order2.OrderProducts.Add(orderProduct2);
   
            orders.Add(order);
            orders.Add(order);


            _orderService = orderService;
        }
        public async Task<IActionResult> Index()
        {
  
            var orders =await _orderService.GetOrders();
            if(orders ==null)
            {
                orders = new List<Order>();
            }
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> EditOrder(int orderId)
        {
            //TODO: implement GetOrderById()
            var ordersresult = await _orderService.GetOrders();
            var order = ordersresult.FirstOrDefault(c => c.Id == orderId);
            return View(order);
        }
        
        [HttpPost]
        public async Task<IActionResult> EditOrder(Order order)
        {
            var orderresult=await _orderService.UpdateOrder(order.Id, order);
            if(orderresult==null)
            {
                return BadRequest();
            }
            TempData["Success"] = $"Order med id: {order.Id} uppdaterades.";
            return RedirectToAction("Index","Admin");
        }
    }
}
