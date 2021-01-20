using DeliCode.Web.Models;
using DeliCode.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.Web.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public CartController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }
        [HttpGet]
        public async Task<IActionResult> AddAsync(Guid Id)
        {

            var product = await _productService.Get(Id);
            if (product == null)
            {
                return BadRequest("produkten fanns inte");
            }

            Cart cart = await _cartService.AddProductToCart(Id);
          
            if (cart == null)
            {
                return BadRequest("produkten kunde inte läggas till");
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult Delete()
        {
            Response.Cookies.Delete("Delicode.CartCookie");
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            Cart cart = await _cartService.GetCart();

            return Ok(cart);
        }
    }
}
