using DeliCode.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliCode.Web.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAll();
        Task<Product> Get(Guid id);
        Task<Product> Remove(Guid id);
        Task<Product> Add(Product product);
        Task<Product> Update(Product product);
        Task<bool> UpdateInventoryAmount(List<OrderProduct> orderProducts);
    }
}