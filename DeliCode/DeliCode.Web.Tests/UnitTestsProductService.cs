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

namespace DeliCode.Web.Tests
{
    public class UnitTestsProductService
    {
        private readonly Mock<IProductService> _productService;
        private readonly Product product;
        private readonly List<Product> productList;

        public UnitTestsProductService()
        {
            _productService = new Mock<IProductService>();
            productList = new List<Product>();
            product = new Product()
            {
                Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Chokladtårta",
                Price = 12.50m,
                Description = "En jättegod tårta"
            };
            var product2 = new Product()
            {
                Id = new Guid("55667788-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Red velvet cake",
                Price = 29.50m,
                Description = "Så jävla gott"
            };
            productList.Add(product2);
            productList.Add(product);
        }

        //GetAll()
        [Fact]
        public async Task GetAllProductsShouldReturnListOfProducts()
        {
            var products = new List<Product>()
            {
                product
            };
            _productService.Setup(productservice => productservice.GetAll()).ReturnsAsync(products);
            var actual = await _productService.Object.GetAll();
            Assert.IsType<List<Product>>(actual);
        }

        [Fact]
        public async Task GetProductByIdShouldReturnProduct()
        {

            Guid id = product.Id;
            _productService.Setup(productservice => productservice.Get(id)).ReturnsAsync(product);
            var actual = await _productService.Object.Get(id);
            Assert.IsType<Product>(actual);
            Assert.Equal(id, actual.Id);
            Assert.Equal(product, actual);
        }

        [Fact]
        public async Task RemoveProductShouldReturnRemovedProduct()
        {
            Guid id = product.Id;
            _productService.Setup(productservice => productservice.Remove(id)).ReturnsAsync(product);
            var actual = await _productService.Object.Remove(id);
            Assert.IsType<Product>(actual);
            Assert.Equal(id, actual.Id);
            Assert.Equal(product, actual);
        }

        [Fact]
        public async Task RemoveProductThatDoesNotExist()
        {
            Guid id = Guid.NewGuid();
            _productService.Setup(productservice => productservice.Remove(id)).ReturnsAsync((Product)null);
            var actual = await _productService.Object.Remove(id);
            Assert.Null(actual);
        }

        [Fact]
        public async Task AddProductShouldReturnAddedProduct()
        {
            _productService.Setup(productservice => productservice.Add(product)).ReturnsAsync(product);
            var actual = await _productService.Object.Add(product);
            Assert.Equal(product, actual);
        }
    }
}
