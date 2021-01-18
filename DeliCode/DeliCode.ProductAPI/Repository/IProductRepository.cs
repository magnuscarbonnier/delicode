using DeliCode.Library.Models;
using DeliCode.ProductAPI.Data;
using DeliCode.ProductAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.ProductAPI.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProduct(Guid Id);
        Task<Product> AddProduct(Product product);
        Task<Product> DeleteProduct(Guid Id);
        Task<Product> UpdateProduct(Product product);
        Task<bool> UpdateInventoryQuanties(Dictionary<Guid, int> productQuantityValuePairs);
    }
}