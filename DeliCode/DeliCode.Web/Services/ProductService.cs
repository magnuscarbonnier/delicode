using DeliCode.Library.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        public async Task<Product> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"product/{id}");
            var productResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Product>(productResponse);
        }
    }
}
