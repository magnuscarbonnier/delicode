using DeliCode.Web.Models;
using DeliCode.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;

        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId);
            return View("ConfirmOrder" , order);
        }
        [HttpGet]
        public IActionResult ShipmentAddress()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ShipmentAddress(string address, string zipCode, string phoneNumber)
        {
            return RedirectToAction("ConfirmOrder", "Order");
        }
    }
}
