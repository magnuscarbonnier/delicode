using DeliCode.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliCode.Web.Services
{
    public interface IInventoryService
    {
        Task<Order> FinalizeOrder(Order order);
        Task<bool> ValidateInventory(List<OrderProduct> orderProducts);
    }
}