using DeliCode.Web.Models;
using DeliCode.Web.Repository;
using DeliCode.Web.Services;
using System.Threading.Tasks;

namespace DeliCode.Web.Tests
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Order> GetOrderById(int? id)
        {
            var order = await _repository.GetOrderById(id);
            return order;
        }

        public async Task<Order> PlaceOrder(Order order)
        {
            order = await _repository.PlaceOrder(order);
            return order;
        }

        public async Task<Order> UpdateOrder(int orderId, Order order)
        {
            if(orderId!=order.Id)
            {
                return null;
            }
            order = await _repository.UpdateOrder(order);
            return order;
        }
    }
}