using DeliCode.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DeliCode.Web.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly HttpClient _httpClient;
        public ProductRepository(HttpClient httpClient)
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

        public async Task<Product> Remove(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/products/{id}");
            var productResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Product>(productResponse);
        }

        public async Task<Product> Add(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync<Product>($"/api/products/",product);
            var productResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Product>(productResponse);
        }

        public async Task<Product> Update(Product product)
        {
            var response = await _httpClient.PutAsJsonAsync<Product>($"api/products/{product.Id}", product);
            var orderResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Product>(orderResponse);
        }

        public async Task<bool> UpdateInventoryAmount(Dictionary<Guid, int> productsKeyValuePairs)
        {
            var response = await _httpClient.PutAsJsonAsync<Dictionary<Guid, int>>($"api/products/update", productsKeyValuePairs);
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<bool>(content);
        }
    }
}
