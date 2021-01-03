using DeliCode.OrderAPI.Data;
using DeliCode.OrderAPI.Models;
using DeliCode.OrderAPI.Repository;
using DeliCode.OrderAPI.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliCode.OrderAPI.Tests
{
    public class UnitTestsOrderAPI
    {
        private readonly MockOrderRepository repos;

        public UnitTestsOrderAPI()
        {
            repos = new MockOrderRepository();
        }

        [Fact]
        public async Task GetAllOrders_ShouldReturnListOfOrders()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("api/order/getallorders");
                var responseString = await response.Content.ReadAsStringAsync();
                var actual = JsonConvert.DeserializeObject<List<Order>>(responseString);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.IsType<List<Order>>(actual);
            }
        }

        [Fact]
        public async Task GetAllOrdersByUserId_ShouldReturnListOfOrders()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync($"api/order/getallordersbyuserid?id={"11223344-5566-7788-99AA-BBCCDDEEFF00"}");
                string responseString = await response.Content.ReadAsStringAsync();
                var actual = JsonConvert.DeserializeObject<List<Order>>(responseString);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.IsType<List<Order>>(actual);

            };
        }
        [Fact]
        public async Task GetSingleOrderByOrderId_ShouldReturnSingleOrder()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync($"api/order/getsingleorderbyorderid?id={"fb6f6dd2-f6c5-4893-ab35-03167f6ebe28"}");
                string responseString = await response.Content.ReadAsStringAsync();
                var actual = JsonConvert.DeserializeObject<Order>(responseString);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("Marie", actual.FirstName);
            }
        }

        [Fact]
        public async Task AddOrder_ShouldReturnNewOrder()
        {
            Order order = new Order
            {
                FirstName = "TestOrder",
                OrderDate = new DateTime(2020, 11, 09),
                Status = Library.Models.OrderStatus.Delivered
            };

            using (var client = new TestClientProvider().Client)
            {
                // Arrange
                var beforeResponse = await client.GetStringAsync("api/order/getallorders");
                int countBefore = JsonConvert.DeserializeObject<List<Order>>(beforeResponse).Count;
                string json = JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/order/AddOrder", content);

                // Act
                var afterResponse = await client.GetStringAsync("api/order/getallorders");
                int actual = JsonConvert.DeserializeObject<List<Order>>(afterResponse).Count;

                var responseString = await response.Content.ReadAsStringAsync();
                var getOrder = JsonConvert.DeserializeObject<Order>(responseString);
                await client.DeleteAsync($"api/order/deletesingleorder?id={getOrder.Id}");

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(countBefore + 1, actual);
            }


        }
        [Fact]
        public async Task DeleteSingleOrder_ShouldDeleteOrderFromList()
        {
            // Create Order to delete
            Order order = new Order
            {
                FirstName = "TestOrder",
                OrderDate = new DateTime(2020, 11, 09),
                Status = Library.Models.OrderStatus.Delivered
            };

            using (var client = new TestClientProvider().Client)
            {
                var json = JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var created = await client.PostAsync("api/order/AddOrder", content);

                string responseString = await created.Content.ReadAsStringAsync();
                var getOrder = JsonConvert.DeserializeObject<Order>(responseString);

                var delete = await client.DeleteAsync($"api/order/deletesingleorder?id={getOrder.Id}");

                Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);
            }
        }
        [Fact]
        public async Task UpdateSingleOrder_ShouldReturnUpdatedOrder()
        {
            Order order = new Order
            {
                FirstName = "TestOrder",
                OrderDate = new DateTime(2020, 11, 09),
                Status = Library.Models.OrderStatus.Delivered
            };

            Order updatedOrder = new Order
            {
                FirstName = "UpdatedOrder",
                OrderDate = new DateTime(2020, 12, 12),
                Status = Library.Models.OrderStatus.Refunded
            };

            using (var client = new TestClientProvider().Client)
            {
                string json = JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var postResponse = await client.PostAsync("api/order/AddOrder", content);
                string postString = await postResponse.Content.ReadAsStringAsync();
                var getOrder = JsonConvert.DeserializeObject<Order>(postString);
                updatedOrder.Id = getOrder.Id;
                string upJson = JsonConvert.SerializeObject(updatedOrder);
                var updatedContent = new StringContent(upJson, Encoding.UTF8, "application/json");

                var updateResponse = await client.PutAsync($"api/order/UpdateSingleOrder?id={getOrder.Id}", updatedContent);
                await client.DeleteAsync($"api/order/deletesingleorder?id={updatedOrder.Id}");

                Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);
            }
        }
    }
}
