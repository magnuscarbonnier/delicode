using DeliCode.Library.Models;
using DeliCode.OrderAPI.Data;
using DeliCode.OrderAPI.Models;
using DeliCode.OrderAPI.Repository;
using DeliCode.OrderAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly OrderDbContext _context;
        private readonly IOrderService _service;

        public OrderController(OrderDbContext context, IOrderService service)
        {
            _context = context;
            _service = service;

        }

        // GET api/Order
        [HttpGet("{id}")]
        public IActionResult GetAllOrdersByUserId(string userId)
        {
            var orders = _service.GetAllOrdersByUserId(userId);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public IActionResult GetSingleOrderByOrderId(Guid id)
        {
            Order order = _service.GetOrderById(id);
            return Ok(order);
        }

        [HttpPost]
        public IActionResult AddOrder([FromBody]Order order)
        {
            _service.AddOrder(order);
            return CreatedAtAction("AddOrder", new { Id = order.Id }, order);
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
