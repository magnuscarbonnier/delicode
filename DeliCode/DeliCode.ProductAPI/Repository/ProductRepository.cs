using DeliCode.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DeliCode.ProductAPI.Tests")]

namespace DeliCode.ProductAPI.Repository
{
    public class ProductRepository
    {
        internal List<Product> Products = new List<Product>()
        {
            new Product() {Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"), Name = "´Kanelbulle", Description = "", Price = 10, ImageUrl = "#"},
            new Product() {Id = Guid.NewGuid(), Name = "Kladdkarta", Description = "", Price = 70, ImageUrl = "#"},
            new Product() {Id = Guid.NewGuid(), Name = "Tårta", Description = "", Price = 89, ImageUrl = "#"},
            new Product() {Id = Guid.NewGuid(), Name = "Cheesecake", Description = "", Price = 29, ImageUrl = "#"},
            new Product() {Id = Guid.NewGuid(), Name = "Muffin", Description = "", Price = 19, ImageUrl = "#"}
        };

        public List<Product> GetAllProducts()
        {
            List<Product> productList = Products;
            return productList;
        }
        public Product GetProduct(Guid Id)
        {
            Product product = Products.Where(e => e.Id == Id).SingleOrDefault();
            return product;
        }
    }
}
