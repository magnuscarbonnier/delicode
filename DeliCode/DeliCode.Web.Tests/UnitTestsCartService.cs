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
using DeliCode.Web.Repository;

namespace DeliCode.Web.Tests
{
    public class UnitTestsCartService
    {
        private readonly MockCartRepository _repository;
        private readonly MockProductRepository _mockProductRepository;
        private readonly ICartService _cartService;

        public UnitTestsCartService()
        {
            _repository = new MockCartRepository();
            _mockProductRepository = new MockProductRepository();
            IProductService _productService = new ProductService(_mockProductRepository);
            _cartService = new CartService(_repository,_productService);
        }
        [Fact]
        public async Task GetCart_ReturnsCart()
        {
            var cart= await _cartService.GetCart();

            Assert.IsType<Cart>(cart);
            Assert.NotEqual(Guid.Empty, cart.SessionId);
        }
        [Fact]
        public async Task GetCart_ReturnsSessionId()
        {
            var result = await _cartService.GetCart();
            var sessionId = result.SessionId;

            Assert.IsType<Guid>(sessionId);
            Assert.NotEqual(Guid.Empty, sessionId);
        }
        [Fact]
        public async Task GetCart_ReturnsListOfCartItems()
        {            
            var result = await _cartService.GetCart();
            var cartItemsResult = result.Items;

            Assert.IsType<List<CartItem>>(cartItemsResult);
        }

        [Fact]
        public async Task GetCart_AddMoreItemsThanInDB_ReturnsCartWithAllDBProducts()
        {
            var product = _mockProductRepository.products.FirstOrDefault();
            product.AmountInStorage = 1;
            _repository._cart.Items.Clear();
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
            var product = _mockProductRepository.products.FirstOrDefault();
            product.AmountInStorage = 0;
            _repository._cart.Items.Clear();
            await _cartService.AddProductToCart(product.Id);

            var cart = await _cartService.GetCart();

            Assert.Empty(cart.Items);
        }


        [Fact]
        public async Task AddProductToCart_ProductAlreadyInCart_ReturnsCartWithIncreasedQuantity()
        {
            var cart = await _repository.GetCart(Guid.NewGuid());
            var cartItem = cart.Items.FirstOrDefault();
            var quantity = cartItem.Quantity+1;

            var result = await _cartService.AddProductToCart(cartItem.Product.Id);
            var cartResult = result.Items;

            Assert.IsType<Cart>(result);
            Assert.Equal(cartItem.Product.Id, cartResult.FirstOrDefault().Product.Id);
            Assert.Equal(quantity, cartResult.FirstOrDefault().Quantity);

        }
        [Fact]
        public async Task AddProductToCart_ProductNotInCart_ReturnsCartWithNewProduct()
        {
            var cart = await _repository.GetCart(Guid.NewGuid());
            var cartItem = cart.Items.FirstOrDefault();
            var product =  new Product { Id = cartItem.Product.Id };
            cart.Items.Clear();
            var result = await _cartService.AddProductToCart(product.Id);
            var cartResult = result.Items;
            var cartresultproduct = cartResult.FirstOrDefault(x => x.Product.Id == product.Id).Product;

            Assert.IsType<Cart>(result);
            Assert.Equal(product.Id, cartresultproduct.Id);
            Assert.Equal(1, cartResult.FirstOrDefault(x => x.Product.Id == product.Id).Quantity);
        }
        [Fact]
        public async Task GetCart_Twice_ReturnsSameCart()
        {
            var firstcart = await _cartService.GetCart();
            var secondCart= await _cartService.GetCart();
            
            Assert.Equal(firstcart.Total, secondCart.Total);
            Assert.Equal(firstcart.Items.Count, secondCart.Items.Count);
            Assert.Equal(firstcart.SessionId, secondCart.SessionId);
        }
    }
}

