using DeliCode.ProductAPI.Controllers;
using DeliCode.ProductAPI.Data;
using DeliCode.ProductAPI.Models;
using DeliCode.ProductAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DeliCode.ProductAPI.Tests
{
    public class UnitTestProductController
    {
        private readonly ProductsController productsController;
        private readonly IProductRepository _repository;
        private readonly List<Product> _products;
        protected DbContextOptions<ProductDbContext> ContextOptions { get; }
        public UnitTestProductController()
        {
            _products = new List<Product>
                {
                    new Product() { Name = "Kanelbulle", Description = "", Price = 10, ImageUrl = "#" },
                    new Product() { Name = "Kladdkaka", Description = "", Price = 70, ImageUrl = "#" },
                    new Product() { Name = "Tårta", Description = "", Price = 89, ImageUrl = "#" },
                    new Product() { Name = "Cheesecake", Description = "", Price = 29, ImageUrl = "#" },
                    new Product() { Name = "Muffin", Description = "", Price = 19, ImageUrl = "#" }
                };
            ContextOptions = SetMockDatabaseOptions();
            SeedMockData();

            _repository = new ProductRepository(new ProductDbContext(ContextOptions));
            productsController = new ProductsController(_repository);
        }
        private DbContextOptions<ProductDbContext> SetMockDatabaseOptions()
        {
            string connectionString = @"Server=(localdb)\mssqllocaldb;Database=ProductApiControllerTestDB;ConnectRetryCount=0";
            return new DbContextOptionsBuilder<ProductDbContext>()
                .UseSqlServer(connectionString)
                .Options;
        }
        private void SeedMockData()
        {
            using (var context = new ProductDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();
            }
        }
        [Fact]
        public async Task GetAllProducts_ShouldReturnAllProducts()
        {

            var result = await productsController.GetProducts();

            var okobjectresult = result.Result as OkObjectResult;
            var products = okobjectresult.Value as List<Product>;

            Assert.IsType<OkObjectResult>(okobjectresult);
            Assert.Equal(products.Count(), _products.Count());
        }
        [Fact]
        public async Task GetAllProducts_NoProductsAdded_ShouldReturnNoContent()
        {
            var products = await _repository.GetAllProducts();
            foreach (var product in products)
            {
                await _repository.DeleteProduct(product.Id);
            }

            var result = await productsController.GetProducts();
            var nocontentresult = result.Result as NoContentResult;

            Assert.IsType<NoContentResult>(nocontentresult);
        }
        [Fact]
        public async Task GetProductById_ShouldReturnProduct()
        {
            var products = await _repository.GetAllProducts();
            var productid = products.FirstOrDefault().Id;

            var result = await productsController.GetProduct(productid);
            var okobjectresult = result.Result as OkObjectResult;
            var product = okobjectresult.Value as Product;

            Assert.IsType<OkObjectResult>(okobjectresult);
            Assert.Equal(productid, product.Id);
        }
        [Fact]
        public async Task GetProductByInvalidId_ShouldReturnNotFound()
        {
            var id = Guid.Empty;

            var result = await productsController.GetProduct(id);
            var notfoundresult = result.Result as NotFoundResult;

            Assert.IsType<NotFoundResult>(notfoundresult);
        }
        [Fact]
        public async Task AddProduct_ShouldReturnNewProductId()
        {
            var result = await productsController.PostProduct(_products.FirstOrDefault()) as CreatedAtActionResult;
            var product = result.Value as Product;

            Assert.Equal(product.Id, _products.FirstOrDefault().Id);
        }
        [Fact]
        public async Task UpdateProduct_ShouldReturnUpdatedOrder()
        {
            var products = await _repository.GetAllProducts();
            var updatedProduct = products.FirstOrDefault();
            updatedProduct.Name = "TestTårta";

            var result = await productsController.PutProduct(updatedProduct.Id, updatedProduct);
            var okobjectresult = result as OkObjectResult;
            var actualProduct = okobjectresult.Value as Product;

            Assert.IsType<OkObjectResult>(okobjectresult);
            Assert.Equal(updatedProduct.Name, actualProduct.Name);
        }
        [Fact]
        public async Task UpdateProduct_InvalidId_ReturnsBadRequest()
        {
            var updatedProduct = _products.FirstOrDefault();
            updatedProduct.Name = "TestTårta";

            var result = await productsController.PutProduct(Guid.NewGuid(), updatedProduct);

            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task UpdateProduct_ProductNotExistsInDataBase_ReturnsNotFound()
        {
            var updatedProduct = new Product { Id = Guid.NewGuid() };

            var result = await productsController.PutProduct(updatedProduct.Id, updatedProduct);

            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task DeleteProduct_ReturnsDeletedProduct()
        {
            var productInDb = await _repository.GetAllProducts();
            var productId = productInDb.FirstOrDefault().Id;

            var result = await productsController.DeleteProduct(productId);
            var okobjectresult = result.Result as OkObjectResult;
            var product = okobjectresult.Value as Product;

            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(product.Id, productId);
        }
        [Fact]
        public async Task DeleteProductThatDoesntExist_ReturnsBadRequest()
        {
            var productId = Guid.NewGuid();

            var result = await productsController.DeleteProduct(productId);
            var badrequestresult = result.Result as BadRequestResult;

            Assert.IsType<BadRequestResult>(badrequestresult);
        }
    }
}