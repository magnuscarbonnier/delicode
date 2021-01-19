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
    public class OrderRepository : IOrderRepository
    {
        private readonly HttpClient _httpClient;
        public OrderRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
            var response = await _httpClient.PostAsJsonAsync<Order>("api/orders", order,options);
            var orderResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Order>(orderResponse);
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            var response = await _httpClient.PutAsJsonAsync<Order>($"api/orders/{order.Id}",order);
            var orderResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Order>(orderResponse);
        }
    }
}
