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
using DeliCode.Web.Repository;

namespace DeliCode.Web.Tests
{
    public class UnitTestsProductService
    {
        private readonly IProductService _productService;
        private readonly MockProductRepository _repository;

        public UnitTestsProductService()
        {
            _repository = new MockProductRepository();
            _productService = new ProductService(_repository);
        }

        [Fact]
        public async Task GetAllProductsShouldReturnListOfProducts()
        {
            var products = await _productService.GetAll();
           
            Assert.IsType<List<Product>>(products);
            Assert.NotEmpty(products);
        }

        [Fact]
        public async Task GetAllProducts_NoProductsInDb_ShouldReturnEmptyListOfProducts()
        {
            _repository.products.Clear();

            var result = await _productService.GetAll();

            Assert.IsType<List<Product>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetProductByIdShouldReturnProduct()
        {
            var products = await _repository.GetAll();
            var product = products.FirstOrDefault();

            var result = await _productService.Get(product.Id);
           
            Assert.IsType<Product>(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(product.Id, result.Id);
        }

        [Fact]
        public async Task GetProductById_ProductNotExists_ShouldReturnNull()
        {
            var product = await _productService.Get(Guid.NewGuid());

            Assert.Null(product);
        }
    }
}
