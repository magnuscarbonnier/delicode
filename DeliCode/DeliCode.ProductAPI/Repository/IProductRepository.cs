using DeliCode.Library.Models;
using DeliCode.ProductAPI.Data;
using DeliCode.ProductAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.ProductAPI.Repository
{
    interface IProductRepository
    {
        List<Product> GetAllProducts();
        Product GetProduct(Guid Id);
        List<Product> AddProduct(Product product);
        List<Product> DeleteProduct(Guid Id);
    }
}
