using DeliCode.Web.Services;
using DeliCode.Library.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAll();
            return View(products);
        }
        public IActionResult Details(Guid productId)
        {
            //var product = _productService.Get(productId);
            //return View(product);
            Product product = new Product
            {
                AmountInStorage = 2,
                Description = "Fina fisken",
                Id = Guid.NewGuid(),
                ImageUrl = "https://picsum.photos/286/180",
                Name = "Fisk",
                Price = 22.9M
            };
            return View(product);
        }
        [HttpPost]
        public IActionResult AddToCart([FromForm] Guid Id)
        {
            //TODO: AddToCart, not just return guid to okresult
            return Ok(Id);
        }
    }
}
