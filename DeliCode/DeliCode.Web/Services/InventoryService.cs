using DeliCode.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.Web.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        public InventoryService(IProductService productService, IOrderService orderSerice)
        {
            _orderService = orderSerice;
            _productService = productService;
        }

        public async Task<Order> FinalizeOrder(Order order)
        {
            bool inventoryStatusValid = await ValidateInventory(order.OrderProducts);

            if (!inventoryStatusValid)
                return null;

            var orderResult = await _orderService.PlaceOrder(order);

            if (orderResult == null)
                return null;

            bool updateSuccess = await _productService.UpdateInventoryAmount(orderResult.OrderProducts);

            if (!updateSuccess)
                await _orderService.DeleteOrder(orderResult.Id);

            return orderResult;
        }

        private async Task<bool> ValidateInventory(List<OrderProduct> orderProducts)
        {
            bool isSuccessful;
            foreach (var orderProduct in orderProducts)
            {
                var product = await _productService.Get(orderProduct.ProductId);

                if (product == null || product.AmountInStorage < orderProduct.Quantity)
                {
                    isSuccessful = false;
                    return isSuccessful;
                }
            }
            isSuccessful = true;
            return isSuccessful;
        }


    }
}
