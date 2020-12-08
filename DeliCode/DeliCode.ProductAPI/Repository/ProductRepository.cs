using DeliCode.ProductAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DeliCode.ProductAPI.Tests")]

namespace DeliCode.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        internal List<Product> products = new List<Product>()
        {
            new Product() {Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"), Name = "´Kanelbulle", Description = "", Price = 10, ImageUrl = "#"},
            new Product() {Id = Guid.NewGuid(), Name = "Kladdkarta", Description = "", Price = 70, ImageUrl = "#"},
            new Product() {Id = Guid.NewGuid(), Name = "Tårta", Description = "", Price = 89, ImageUrl = "#"},
            new Product() {Id = Guid.NewGuid(), Name = "Cheesecake", Description = "", Price = 29, ImageUrl = "#"},
            new Product() {Id = Guid.NewGuid(), Name = "Muffin", Description = "", Price = 19, ImageUrl = "#"}
        };

        public List<Product> GetAllProducts()
        {
            List<Product> productList = products;
            return productList;
        }
        public Product GetProduct(Guid Id)
        {
            Product product = products.Where(e => e.Id == Id).SingleOrDefault();
            return product;
        }
        public List<Product> AddProduct(Product product)
        {
            products.Add(product);
            return products;
        }
        public List<Product> DeleteProduct(Guid Id)
        {
            var deleteProduct = products
                .Where(e => e.Id == Id)
                .SingleOrDefault();

            products.Remove(deleteProduct);
            return products;
        }
    }
}
