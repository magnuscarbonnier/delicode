using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliCode.Library.Models;
using DeliCode.ProductAPI.Data;
using DeliCode.ProductAPI.Repository;
using System.Transactions;

namespace DeliCode.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductDbContext _context;
        ProductRepository repos = new ProductRepository();

        public ProductsController(ProductDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            List<Product> productList = repos.GetAllProducts();
            return Ok(productList);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public IActionResult GetProduct(Guid id)
        {
            Product product = repos.GetProduct(id);
            return Ok(product);
        }

        // POST: api/Products
        [HttpPost]
        public IActionResult PostProduct([FromBody] Product product)
        {
            using (var scope = new TransactionScope())
            {
                repos.AddProduct(product);
                scope.Complete();
                return CreatedAtAction("PostProduct", new { Id = product.Id }, product);
            }
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(Guid id)
        {
            var productList = repos.DeleteProduct(id);
            return Ok(productList);
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
