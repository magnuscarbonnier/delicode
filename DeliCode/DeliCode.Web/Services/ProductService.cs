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

        public async Task<bool> UpdateInventoryAmount(List<OrderProduct> orderProducts)
        {
            var productsKeyValuePairs = MapOrderProductsToDictionary(orderProducts);

            var response = await _httpClient.PutAsJsonAsync<Dictionary<Guid, int>>($"https://localhost:44333/api/products/update", productsKeyValuePairs);
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<bool>(content);
        }

        private Dictionary<Guid, int> MapOrderProductsToDictionary(List<OrderProduct> orderProducts)
        {
            Dictionary<Guid, int> productQuantityValuePairs = new Dictionary<Guid, int>();
            foreach (var product in orderProducts)
            {
                productQuantityValuePairs.Add(product.Id, product.Quantity);
            }

            return productQuantityValuePairs;
        }
    }
}
