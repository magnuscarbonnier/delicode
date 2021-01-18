using DeliCode.Library.Models;
using DeliCode.OrderAPI.Data;
using DeliCode.OrderAPI.Models;
using DeliCode.OrderAPI.Repository;
using DeliCode.OrderAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DeliCode.OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private IOrderRepository _repository;


        public OrdersController(IOrderRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult> GetOrders()
        {
            var orders = await _repository.GetAllOrders();

            if (orders != null && orders.Any())
            {
                return Ok(orders);
            }
            return NoContent();
        }

        [HttpGet("getbyuserid/{id}")]
        public async Task<ActionResult> GetOrdersByUserId(string id)
        {
            var orders = await _repository.GetAllOrdersByUserId(id);
            if (orders != null && orders.Any())
            {
                return Ok(orders);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderByOrderId(int id)
        {
            var order = await _repository.GetOrderById(id);
            if (order != null)
            {
                return Ok(order);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> AddOrder([FromBody] Order order)
        {
            if (order.OrderProducts == null)
            {
                return BadRequest("order not added");
            }
            var orderResult = await _repository.AddOrder(order);
            if (orderResult != null)
            {
                return CreatedAtAction("AddOrder", orderResult);
            }

            return BadRequest("order not added");
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }
            order = await _repository.UpdateOrder(order);
            if (order == null)
            {
                return BadRequest();
            }
            return Ok(order);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int orderId)
        {
            Order order = await _repository.DeleteOrder(orderId);

            if (order == null)
            {
                return BadRequest();
            }
            return Ok(order);
        }
    }
}
