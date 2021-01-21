using DeliCode.Library.Models;
using DeliCode.ProductAPI.Data;
using DeliCode.ProductAPI.Models;
using DeliCode.ProductAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliCode.ProductAPI.Tests
{
    public class UnitTestProductRepository
    {
        private readonly IProductRepository _repository;
        private readonly Product _product;
        protected DbContextOptions<ProductDbContext> ContextOptions { get; }

        public UnitTestProductRepository()
        {
            ContextOptions = SetMockDatabaseOptions();
            SeedMockData();
            _repository = new ProductRepository(new ProductDbContext(ContextOptions));
            _product = new Product
            {
                ImageUrl = "imgurl",
                Description = "description",
                AmountInStorage = 2,
                Name = "testproduct",
                Price = 3
            };
        }

        private static DbContextOptions<ProductDbContext> SetMockDatabaseOptions()
        {
            string connectionString = @"Server=(localdb)\mssqllocaldb;Database=ProductApiRepositoryTestDB;ConnectRetryCount=0";
            return new DbContextOptionsBuilder<ProductDbContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        private void SeedMockData()
        {
            using var context = new ProductDbContext(ContextOptions);
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }

        [Fact]
        public async Task AddProduct_ReturnsCreatedProductWithId()
        {
            var result = await _repository.AddProduct(_product);

            Assert.IsType<Product>(result);
            Assert.NotEqual(Guid.Empty, result.Id);
        }

        [Fact]
        public async Task AddProduct_ProductIsEmpty_ReturnsProductWithId()
        {
            var result = await _repository.AddProduct(new Product());

            Assert.IsType<Product>(result);
            Assert.NotEqual(Guid.Empty, result.Id);
        }

        [Fact]
        public async Task DeleteProduct_IdNotExists_ReturnsNull()
        {
            var productIdNotInDb = Guid.NewGuid();

            var result = await _repository.DeleteProduct(productIdNotInDb);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteProduct_IdExists_ReturnsDeletedProduct()
        {
            var products= await _repository.GetAllProducts();
            var productIdInDb = products.FirstOrDefault().Id;

            var result = await _repository.DeleteProduct(productIdInDb);
            var tryGetDeletedOrder = await _repository.GetProduct(productIdInDb);

            Assert.IsType<Product>(result);
            Assert.Equal(productIdInDb, result.Id);
            Assert.Null(tryGetDeletedOrder);
        }

        [Fact]
        public async Task GetProductById_IdExists_ReturnsProduct()
        {
            var products = await _repository.GetAllProducts();
            var productIdInDb = products.FirstOrDefault().Id;

            var result = await _repository.GetProduct(productIdInDb);

            Assert.IsType<Product>(result);
            Assert.Equal(productIdInDb, result.Id);
        }

        [Fact]
        public async Task GetProductById_IdNotExists_ReturnsNull()
        {
            var productIdNotInDb = Guid.NewGuid();

            var result = await _repository.GetProduct(productIdNotInDb);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsListOfOrder()
        {
            var productCountInDb = 5;
            var result = await _repository.GetAllProducts();

            Assert.IsType<List<Product>>(result);
            Assert.Equal(productCountInDb, result.Count);
        }

        [Fact]
        public async Task GetAllProducts_EmptyDb_ReturnsEmtyList()
        {
            foreach (var product in await _repository.GetAllProducts())
            {
                await _repository.DeleteProduct(product.Id);
            }

            var result = await _repository.GetAllProducts();

            Assert.IsType<List<Product>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task UpdateProduct_ProductNotExists_ReturnNull()
        {
            var productidnotindb = Guid.NewGuid();
            var product = new Product { Id = productidnotindb, Name="Test"};

            var result = await _repository.UpdateProduct(product);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateProduct_ProductExists_ReturnUpdatedProduct()
        {
            var name = "Testproduct";
            var products = await _repository.GetAllProducts();
            var product = products.FirstOrDefault();
            product.Name = name;

            var result = await _repository.UpdateProduct(product);

            Assert.Equal(name, result.Name);
            Assert.IsType<Product>(result);
            Assert.Equal(product.Id, result.Id);
        }
    }
}
