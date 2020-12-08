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

namespace DeliCode.Web.Tests
{
    public class UnitTestsCartService
    {
        private readonly Mock<ICartService> _cartService;
        private readonly Product product;
        private readonly Cart cart;

        public UnitTestsCartService()
        {
            _cartService = new Mock<ICartService>();
            product = new Product()
            {
                Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Chokladtårta",
                Price = 12.50m,
                Description = "En jättegod tårta"
            };
            
        }

        [Fact]
        void GetCartShouldReturnCart()
        {
            //Act
            var products = new List<Product>()
            {
                product
            };
            var emptycart = new Cart();
            _cartService.Setup(cartservice => cartservice.GetCart()).Returns(emptycart);
            var actual =  _cartService.Object.GetCart();

            //Assert
            Assert.IsType<Cart>(actual);
        }

        [Fact]
        void AddProductReturnsCartWithProduct()
        {
            //Act
            var products = new List<Product>()
            {
                product
            };
            _cartService.Setup(cartservice => cartservice.AddProductToCart(product)).Returns(cart);
            var actual = _cartService.Object.AddProductToCart(product);

            //Assert
            Assert.Equal(product.Id, actual.Items.FirstOrDefault(c => c.Product.Id == product.Id).Product.Id);
        } 
    }
}

