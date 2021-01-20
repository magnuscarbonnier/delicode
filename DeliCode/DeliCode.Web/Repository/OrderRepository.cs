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
    public class OrderRepository : IOrderRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ApiToken _token;
        public OrderRepository(HttpClient httpClient)
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
            request.AddParameter("application/json", "{\"client_id\":\"2voc9DFGIvDuoriLq4XOMJz6dRoJ2ZlQ\",\"client_secret\":\"_OGxZx6u34VVADFmaAI8YZMtVBtbn9GPfkQL0kNgbtNLuPp3UwFyirY5ca4yC0T4\",\"audience\":\"https://localhost:44399/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<ApiToken>(response.Content);
        }

        public Task<Order> DeleteOrder(int orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Order>> GetAll()
        {
            var response = await _httpClient.GetAsync("api/orders");
            var orderResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Order>>(orderResponse);
        }

        public async Task<Order> GetOrderById(int id)
        {
            var response = await _httpClient.GetAsync($"api/orders/{id}");
            var orderResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Order>(orderResponse);
        }

        public async Task<List<Order>> GetOrdersByUsersId(string userId)
        {
            var response = await _httpClient.GetAsync($"api/orders/getbyuserid/{userId}");
            var orderResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Order>>(orderResponse);
        }

        public async Task<Order> PlaceOrder(Order order)
        {
            JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            var response = await _httpClient.PostAsJsonAsync<Order>("api/orders", order, options);
            var orderResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Order>(orderResponse);
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            var response = await _httpClient.PutAsJsonAsync<Order>($"api/orders/{order.Id}", order);
            var orderResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Order>(orderResponse);
        }
    }
}
