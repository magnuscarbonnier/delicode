using DeliCode.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
        public Task<Order> DeleteOrder(int? orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Order>> GetAll()
        {
            var response = await _httpClient.GetAsync("api/order/getorders");
            var orderResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Order>>(orderResponse);
        }

        public Task<Order> GetOrderById(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetOrdersByUsersId(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Order> PlaceOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public Task<Order> UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
