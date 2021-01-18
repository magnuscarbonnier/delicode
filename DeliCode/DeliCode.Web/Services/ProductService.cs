using DeliCode.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DeliCode.Web.Services
{
    public class ProductService : IProductService
    {

        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Product>> GetAll()
        {
            var response = await _httpClient.GetAsync("/api/products");
            var productResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Product>>(productResponse);
        }

        public async Task<Product> Get(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/products/{id}");
            var productResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Product>(productResponse);
        }

        public Task<Product> Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> Add(Product product)
        {
            throw new NotImplementedException();
        }
        //TODO productresponse
        public async Task<Product> Update(Product product)
        {
            var response = await _httpClient.PutAsJsonAsync<Product>($"api/products/{product.Id}", product);
            var orderResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Product>(orderResponse);
        }
    }
}
