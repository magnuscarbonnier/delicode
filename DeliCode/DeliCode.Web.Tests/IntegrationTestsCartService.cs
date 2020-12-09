using DeliCode.Web.Models;
using DeliCode.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliCode.Web.Tests
{
    public class IntegrationTestsCartService
    {
        IHttpContextAccessor _mockhttpContextAccessor;
        ICartService _cartService;
        private Product product;
        public IntegrationTestsCartService()
        {
      
            product = new Product()
            {
                Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Chokladtårta",
                Price = 12.50m,
                Description = "En jättegod tårta"
            };
            _mockhttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            _cartService = new CartService(_mockhttpContextAccessor);
        }

        [Fact]
        void AddProductToEmptyCart_ReturnsCartWithProduct()
        {
            //Arrange
            var expectedProduct = product;
            var expectedProductCount = 1;

            //Act
            var cartResult = _cartService.AddProductToCart(product);

            //Assert
            Assert.Equal(expectedProduct, cartResult.Items.FirstOrDefault(x => x.Product.Id == product.Id).Product);
            Assert.Equal(expectedProductCount, cartResult.Items.FirstOrDefault(x => x.Product.Id == product.Id).Quantity);
        }
    }
}