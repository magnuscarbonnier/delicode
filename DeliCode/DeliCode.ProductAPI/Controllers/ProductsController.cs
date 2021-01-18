using DeliCode.ProductAPI.Data;
using DeliCode.ProductAPI.Models;
using DeliCode.ProductAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.ProductAPI.Controllers
{
    [Route("api/[controller]")]    
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _repository.GetAllProducts();
            if (products != null && products.Any())
            {
                return Ok(products);
            }
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _repository.GetProduct(id);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateInventory(Dictionary<Guid, int> productQuantityValuePairs)
        {
            bool updateIsSuccessfull = await _repository.UpdateInventoryQuanties(productQuantityValuePairs);

            if (updateIsSuccessfull)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(Guid id,[FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            product = await _repository.UpdateProduct(product);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> PostProduct(Product product)
        {
            var productToReturn = await _repository.AddProduct(product);

            return CreatedAtAction("PostProduct", productToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(Guid id)
        {
            Product product = await _repository.DeleteProduct(id);
            if (product == null)
            {
                return BadRequest();
            }
            return Ok(product);
        }
    }
}