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

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.IsType<List<Product>>(actual);
            }
        }
        [Fact]
        public async Task GetProductShouldReturnSingleProduct()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync($"api/products/{ new Guid("F7675644-F5C3-4604-838E-09F4E2A64F10")}");
                string responseString = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Product>(responseString);
                var actual = product.Name;

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("Korvar", actual);
            }
        }
        [Fact]
        public async Task PostProduct_ShouldReturnCreatedProduct()
        {
            Product product = new Product { Name = "TestProduct", Description = "#", Price = 150, ImageUrl = "#" };
            using (var client = new TestClientProvider().Client)
            {
                //Arrange
                var beforeResponse = await client.GetStringAsync("api/products");
                int countBefore = JsonConvert.DeserializeObject<List<Product>>(beforeResponse).Count;
                string json = JsonConvert.SerializeObject(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/products", content);

                //Act
                var afterResponse = await client.GetStringAsync("api/products");
                int actual = JsonConvert.DeserializeObject<List<Product>>(afterResponse).Count;

                var responseString = await response.Content.ReadAsStringAsync();
                var getProduct = JsonConvert.DeserializeObject<Product>(responseString);
                await client.DeleteAsync($"api/products/{getProduct.Id}");

                //Assert
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(countBefore + 1, actual);
            }
        }
        [Fact]
        public async Task DeleteProductShouldDeleteProductFromList()
        {
            Product product = new Product { Name = "TestProduct", Description = "#", Price = 150, ImageUrl = "#" };

            using (var client = new TestClientProvider().Client)
            {
                //First create
                var json = JsonConvert.SerializeObject(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var created = await client.PostAsync("api/products", content);

                string responseString = await created.Content.ReadAsStringAsync();
                var getProduct = JsonConvert.DeserializeObject<Product>(responseString);

                //Then delete
                var delete = await client.DeleteAsync($"api/products/{getProduct.Id}");

                Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);
            }
        }
        [Fact]
        public async Task PutProductShouldUpdateProduct()
        {
            Product product = new Product { Name = "TestProduct", Description = "#", Price = 150, ImageUrl = "#" };
            Product putProduct = new Product { Name = "UpdatedProduct", Description = "Updated", Price = 150, ImageUrl = "Updated" };

            using (var client = new TestClientProvider().Client)
            {
                string json = JsonConvert.SerializeObject(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var postResponse = await client.PostAsync("api/products", content);
                string postString = await postResponse.Content.ReadAsStringAsync();
                var getProduct = JsonConvert.DeserializeObject<Product>(postString);
                putProduct.Id = getProduct.Id;
                string upJson = JsonConvert.SerializeObject(putProduct);
                var updatedContent = new StringContent(upJson, Encoding.UTF8, "application/json");

                var putResponse = await client.PutAsync($"api/products/{getProduct.Id}", updatedContent);
                await client.DeleteAsync($"api/products/{putProduct.Id}");

                Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);
            }
        }
    }
}
