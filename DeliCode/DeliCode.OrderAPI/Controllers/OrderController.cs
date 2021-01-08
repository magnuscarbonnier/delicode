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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : Controller
    {
        private IOrderRepository _repository;


        public OrderController(IOrderRepository repository)
        {
            _repository = repository;
        }

        // GET api/Order

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            return await _repository.GetAllOrders();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrdersByUserId(string id)
        {
            var orders =  _repository.GetAllOrdersByUserId(id);
            return Ok(orders);
        }

        [HttpGet]
        public async Task<ActionResult<Order>> GetSingleOrderByOrderId(Guid id)
        {
            var order = await _repository.GetOrderById(id);
            return order;
        }

        [HttpPost]
        public async Task<ActionResult> AddOrder([FromBody]Order order)
        {
            var orderResult =  await _repository.AddOrder(order);
            if (orderResult == null)
            {
                return BadRequest();
            }
            return CreatedAtAction("AddOrder", orderResult);
        }

        [HttpDelete]
        public async Task<ActionResult<Order>> DeleteSingleOrder(Guid id)
        {
            var order =  _repository.DeleteOrderByOrderId(id);
            if(order == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult<Order>> UpdateSingleOrder(Guid id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _repository.UpdateOrder(order);

            return NoContent();
        }
    }
}
