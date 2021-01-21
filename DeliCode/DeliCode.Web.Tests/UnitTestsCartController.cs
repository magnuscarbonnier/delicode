using DeliCode.Library.Models;
using DeliCode.Web.Controllers;
using DeliCode.Web.Models;
using DeliCode.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliCode.Web.Tests
{
    public class UnitTestsCartController
    {
        private readonly CartController _controller;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly MockCartRepository _cartRepository;
        private readonly MockProductRepository _productRepository;

        public UnitTestsCartController()
        {
            _productRepository = new MockProductRepository();
            _productService = new ProductService(_productRepository);
            _cartRepository = new MockCartRepository();
            _cartService = new CartService(_cartRepository,_productService);
            _controller = new CartController(_productService, _cartService);
        }

        [Fact]
        public async Task AddProductToCart_ProductNotExists_ReturnsBadRequest()
        {
            var idNotInProductDb = Guid.NewGuid();
            var result = await _controller.AddAsync(idNotInProductDb);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AddProductToCart_ProductExistsInProductDb_ReturnsOk()
        {
            var idInProductDb = _productRepository.products.FirstOrDefault().Id;
        
            var result = await _controller.AddAsync(idInProductDb);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task GetCart_ReturnsCart()
        {
            var action = await _controller.GetAsync();
            var result = action as OkObjectResult;
            var actualCart = result.Value as Cart;

            Assert.IsType<Cart>(actualCart);
            Assert.NotEqual(Guid.Empty, actualCart.SessionId);
        }

        [Fact]
        public async Task GetCart_ReturnsOk()
        {
            var result = await _controller.GetAsync();
         
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetCart_AddMoreItemsThanInDB_ReturnsCartWithAllDBProducts()
        {
            var product = _productRepository.products.FirstOrDefault();
            product.AmountInStorage = 1;
            _cartRepository._cart.Items.Clear();
            await _cartService.AddProductToCart(product.Id);
            await _cartService.AddProductToCart(product.Id);
            await _cartService.AddProductToCart(product.Id);

            var cart = await _cartService.GetCart();
            var amount = cart.Items.SingleOrDefault(x => x.Product.Id == product.Id).Quantity;

            Assert.Equal(product.AmountInStorage, amount);
        }

        [Fact]
        public async Task GetCart_AddItemsNotAvailableInDB_ReturnsCartWithoutProduct()
        {
            var product = _productRepository.products.FirstOrDefault();
            product.AmountInStorage = 0;
            _cartRepository._cart.Items.Clear();
            await _cartService.AddProductToCart(product.Id);

            var cart = await _cartService.GetCart();

            Assert.Empty(cart.Items);
        }
    }

}
