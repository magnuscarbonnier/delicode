using DeliCode.Web.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DeliCode.Web.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ApiToken _token;
        public ProductRepository(HttpClient httpClient)
        {
            _token = GetOrderToken();
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_token.TokenType, _token.AccessToken);
        }

        private ApiToken GetOrderToken()
        {
            var client = new RestClient("https://dev-5fnthzy6.eu.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"client_id\":\"2voc9DFGIvDuoriLq4XOMJz6dRoJ2ZlQ\",\"client_secret\":\"_OGxZx6u34VVADFmaAI8YZMtVBtbn9GPfkQL0kNgbtNLuPp3UwFyirY5ca4yC0T4\",\"audience\":\"https://localhost:44333/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<ApiToken>(response.Content);
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
