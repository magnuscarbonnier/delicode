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
        private readonly IProductService _productService;
        private HttpClient _httpClient;

        public UnitTestsCartService()
        {
            _repository = new MockCartRepository();
            
            _product = new Product()
            {
                Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Chokladtårta",
                Price = 12.50m,
                Description = "En jättegod tårta"
            };
            var productAPIUrl = "https://localhost:44333";

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(productAPIUrl);
            _productService = new ProductService(_httpClient);
            _cartService = new CartService(_repository,_productService);
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
            var result = await _cartService.GetCart();
            var cartItemsResult = result.Items;

            Assert.IsType<List<CartItem>>(cartItemsResult);
        }

        [Fact]
        private async Task GetCart_CartItemsCountLessThanProductsInStorage_ReturnsCartWithOnlyProductsAvailable()
        {
            _repository._cart.Items.Clear();
            _repository._cart.Items.Add(new CartItem { Quantity = 3, Product = await _productService.Get(new Guid("4DF795CF-EA1C-47C1-A4E0-F20742CFE359"))});

            var result = await _cartService.GetCart();
            var cartItemsResult = result.Items;
            foreach (var cartitem in cartItemsResult)
            {
                var cartitemamount= cartitem.Quantity;
                var product = await _productService.Get(cartitem.Product.Id);
                var productamount = product.AmountInStorage;
                Assert.InRange(cartitem.Quantity, 1, productamount);
            }
        }

        [Fact]
        private async Task GetCart_CartItemsMoreThanProductsInStorage_ReturnsCartWithOnlyProductsAvailable()
        {
            _repository._cart.Items.Clear();
            _repository._cart.Items.Add(new CartItem { Quantity = 5, Product = await _productService.Get(new Guid("4DF795CF-EA1C-47C1-A4E0-F20742CFE359")) });

            var result = await _cartService.GetCart();
            var cartItemsResult = result.Items;
            foreach (var cartitem in cartItemsResult)
            {
                var cartitemamount = cartitem.Quantity;
                var product = await _productService.Get(cartitem.Product.Id);
                var productamount = product.AmountInStorage;
                Assert.InRange(cartitem.Quantity, 1, productamount);
            }
        }

        [Fact]
        private async Task AddProductToCart_ProductAlreadyInCart_ReturnsCart()
        {
            var product = await _productService.Get(new Guid("4DF795CF-EA1C-47C1-A4E0-F20742CFE359"));
            var cartitems = new List<CartItem> { new CartItem { Product = product, Quantity = 2 } };

            var result = await _cartService.AddProductToCart(product);
            var cartResult = result.Items;

            Assert.IsType<Cart>(result);
            Assert.Equal(product.Id, cartResult.FirstOrDefault().Product.Id);
            Assert.Equal(cartitems.FirstOrDefault().Quantity, cartResult.FirstOrDefault().Quantity);

        }
        //TODO make similiar test
        //[Fact]
        //private async Task AddProductToCart_ProductNotInCart_ReturnsCart()
        //{
        //    _product.Id = Guid.NewGuid();
        //    var cartitems = new List<CartItem> { new CartItem { Product = _product, Quantity = 1 } };

        //    var result = await _cartService.AddProductToCart(_product);
        //    var cartResult = result.Items;
        //    var cartresultproduct = cartResult.FirstOrDefault(x => x.Product.Id == _product.Id).Product;

        //    Assert.IsType<Cart>(result);
        //    Assert.Equal(_product.Id, cartresultproduct.Id);
        //    Assert.Equal(cartitems.FirstOrDefault().Quantity, cartResult.FirstOrDefault(x=>x.Product.Id==_product.Id).Quantity);
        //}
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

