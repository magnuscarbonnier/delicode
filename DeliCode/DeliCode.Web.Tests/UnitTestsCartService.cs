using DeliCode.Web.Models;
using DeliCode.Web.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DeliCode.Library.Models;
using Microsoft.AspNetCore.Http;

namespace DeliCode.Web.Tests
{
    public class UnitTestsCartService
    {
        private readonly Product _product;
        private readonly MockCartRepository _repository;
        private readonly ICartService _cartService;

        public UnitTestsCartService()
        {
            _repository = new MockCartRepository();
            _cartService = new CartService(_repository);
            _product = new Product()
            {
                Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Chokladtårta",
                Price = 12.50m,
                Description = "En jättegod tårta"
            };
        }
        [Fact]
        private async Task GetCart_ReturnsCart()
        {
            var cart= await _cartService.GetCart();

            Assert.IsType<Cart>(cart);
            Assert.NotEqual(Guid.Empty, cart.SessionId);
        }
        [Fact]
        private async Task GetCart_ReturnsSessionId()
        {
            var result = await _cartService.GetCart();
            var sessionId = result.SessionId;

            Assert.IsType<Guid>(sessionId);
            Assert.NotEqual(Guid.Empty, sessionId);
        }
        [Fact]
        private async Task GetCart_ReturnsListOfCartItems()
        {
            var cartitems = new List<CartItem> { new CartItem { Product = _product, Quantity=2 } };
            
            var result = await _cartService.GetCart();
            var cartItemsResult = result.Items;

            Assert.IsType<List<CartItem>>(cartItemsResult);
            Assert.Equal(cartitems.FirstOrDefault().Product.Id, cartItemsResult.FirstOrDefault().Product.Id );
            Assert.Equal(cartitems.FirstOrDefault().Total, cartItemsResult.FirstOrDefault().Total);
        }
        [Fact]
        private async Task AddProductToCart_ProductAlreadyInCart_ReturnsCart()
        {
            var cartitems = new List<CartItem> { new CartItem { Product = _product, Quantity = 3 } };

            var result = await _cartService.AddProductToCart(_product);
            var cartResult = result.Items;

            Assert.IsType<Cart>(result);
            Assert.Equal(_product.Id, cartResult.FirstOrDefault().Product.Id);
            Assert.Equal(cartitems.FirstOrDefault().Quantity, cartResult.FirstOrDefault().Quantity);

        }
        [Fact]
        private async Task AddProductToCart_ProductNotInCart_ReturnsCart()
        {
            _product.Id = Guid.NewGuid();
            var cartitems = new List<CartItem> { new CartItem { Product = _product, Quantity = 1 } };

            var result = await _cartService.AddProductToCart(_product);
            var cartResult = result.Items;
            var cartresultproduct = cartResult.FirstOrDefault(x => x.Product.Id == _product.Id).Product;

            Assert.IsType<Cart>(result);
            Assert.Equal(_product.Id, cartresultproduct.Id);
            Assert.Equal(cartitems.FirstOrDefault().Quantity, cartResult.FirstOrDefault(x=>x.Product.Id==_product.Id).Quantity);
        }
        [Fact]
        private async Task GetCart_Twice_ReturnsSameCart()
        {
            var firstcart = await _cartService.GetCart();
            var secondCart= await _cartService.GetCart();
            
            Assert.Equal(firstcart.Total, secondCart.Total);
            Assert.Equal(firstcart.Items.Count, secondCart.Items.Count);
            Assert.Equal(firstcart.SessionId, secondCart.SessionId);
        }
    }
}

