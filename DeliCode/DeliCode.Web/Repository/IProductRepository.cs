using DeliCode.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.Web.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll();
        Task<Product> Get(Guid id);
        Task<Product> Remove(Guid id);
        Task<Product> Add(Product product);
        Task<Product> Update(Product product);
    }
}
