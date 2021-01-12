using DeliCode.Web.Models;
using DeliCode.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.Web.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View(OrderSummary.Cart);
        }
        public IActionResult ConfirmOrder(string shipment, string payment)
        { 
            if (shipment == "send")
            {
                return RedirectToAction("ShipmentAddress", "Order");
            }
            return View(OrderSummary.Cart);
        }
        public IActionResult ShipmentAddress()
        {
            return View();
        }
    }
}
