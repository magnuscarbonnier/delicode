﻿using DeliCode.Web.Models;
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
        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId);
            Response.Cookies.Delete("Delicode.CartCookie");

            return View("ConfirmOrder" , order);
        }
    }
}
