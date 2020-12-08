using DeliCode.ProductAPI.Models;
using DeliCode.ProductAPI.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;



namespace DeliCode.ProductAPI.Tests
{
    public class UnitTestsProductAPI
    {
        private readonly MockProductRepository repos;
        public UnitTestsProductAPI()
        {
            repos = new MockProductRepository();
        }
        
        [Fact]
        public async Task GetAllProductsShouldReturnListOfProducts()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("api/products");
                var responseString = await response.Content.ReadAsStringAsync();
                var actual = JsonConvert.DeserializeObject<List<Product>>(responseString);
                Assert.IsType<List<Product>>(actual);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
        [Fact]
        public async Task AddNewProduct_ShouldReturnCreatedProduct()
        {
            Product product = new Product { Name = "TestProduct", Description = "#", Price = 150, ImageUrl = "#" };
            using (var client = new TestClientProvider().Client)
            {
                //Arrange
                string json =  JsonConvert.SerializeObject(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Guid productId;

                //Act
                var response = await client.PostAsync("api/products",content);
                var responseString = await response.Content.ReadAsStringAsync();
                var actual = JsonConvert.DeserializeObject<Product>(responseString);
                productId = actual.Id;

                //Assert
                Assert.IsType<Product>(actual);
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            }
        }

        [Fact]
        public void GetProductShouldReturnSingleProduct()
        {
            var expected = repos.products
                .Where(e => e.Id == new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"))
                .SingleOrDefault();
            Product product = repos.GetProduct(new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"));
            Assert.Equal(expected, product);
        }
        [Fact]
        public void AddProductShouldAddNewProduct()
        {
            
            Product product = new Product { Id = Guid.NewGuid(), Name = "NyProduct", Description = "", Price = 150, ImageUrl = "#" };
            int expected = repos.products.Count() + 1;
            int actual = repos.AddProduct(product).Count();
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void DeleteProductShouldDeleteProductFromList()
        {
            
            int expected = repos.products.Count() - 1;
            int actual = repos.DeleteProduct(new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00")).Count();
            Assert.Equal(expected, actual);
        }

        //Tests:

        // PUT: api/Products/5
        // PutProduct(Guid id, Product product)

        // ProductExists(Guid id)
    }
}
