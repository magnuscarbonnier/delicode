using DeliCode.ProductAPI.Controllers;
using DeliCode.ProductAPI.Models;
using DeliCode.ProductAPI.Repository;
using Microsoft.AspNetCore.Mvc;
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
        private readonly MockProductRepository _service;
        private readonly List<Product> _products;

        public UnitTestProductController()
        {
            {
                _products = new List<Product>
                {
                    new Product() { Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"), Name = "Kanelbulle", Description = "", Price = 10, ImageUrl = "#" },
                    new Product() { Id = Guid.NewGuid(), Name = "Kladdkaka", Description = "", Price = 70, ImageUrl = "#" },
                    new Product() { Id = Guid.NewGuid(), Name = "Tårta", Description = "", Price = 89, ImageUrl = "#" },
                    new Product() { Id = Guid.NewGuid(), Name = "Cheesecake", Description = "", Price = 29, ImageUrl = "#" },
                    new Product() { Id = Guid.NewGuid(), Name = "Muffin", Description = "", Price = 19, ImageUrl = "#" }
                };
            };
            _service = new MockProductRepository();
            productsController = new ProductsController(_service);
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
            _service.products.Clear();

            var result = await productsController.GetProducts();
            var nocontentresult = result.Result as NoContentResult;

            Assert.IsType<NoContentResult>(nocontentresult);
        }
        [Fact]
        public async Task GetProductById_ShouldReturnProduct()
        {
            var id = _products.FirstOrDefault().Id;

            var result = await productsController.GetProduct(id);
            var okobjectresult = result.Result as OkObjectResult;
            var product = okobjectresult.Value as Product;

            Assert.IsType<OkObjectResult>(okobjectresult);
            Assert.Equal(product.Id, id);
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
        // TODO Göra ett test för AddOrder som misslyckas
        [Fact]
        public async Task UpdateProduct_ShouldReturnUpdatedOrder()
        {
            var updatedProduct = _products.FirstOrDefault();
            updatedProduct.Name = "TestTårta";

            var result = await productsController.PutProduct(updatedProduct.Id, updatedProduct);
            var okobjectresult = result as OkObjectResult;
            var expectedProduct = okobjectresult.Value as Product;

            Assert.IsType<OkObjectResult>(okobjectresult);
            Assert.Equal(expectedProduct.Name, updatedProduct.Name);
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
            var updatedProduct = _products.FirstOrDefault();
            updatedProduct.Id = new Guid();

            var result = await productsController.PutProduct(updatedProduct.Id, updatedProduct);

            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task DeleteProduct_ReturnsDeletedProduct()
        {
            var productId = _service.products.FirstOrDefault().Id;

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