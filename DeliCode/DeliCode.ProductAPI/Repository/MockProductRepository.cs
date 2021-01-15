//using DeliCode.ProductAPI.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Threading.Tasks;

//[assembly: InternalsVisibleTo("DeliCode.ProductAPI.Tests")]

//namespace DeliCode.ProductAPI.Repository
//{
//    public class MockProductRepository : IProductRepository
//    {
//        internal List<Product> products = new List<Product>()
//        {
//            new Product() {Id = new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"), Name = "Kanelbulle", Description = "", Price = 10, ImageUrl = "#"},
//            new Product() {Id = Guid.NewGuid(), Name = "Kladdkaka", Description = "", Price = 70, ImageUrl = "#"},
//            new Product() {Id = Guid.NewGuid(), Name = "Tårta", Description = "", Price = 89, ImageUrl = "#"},
//            new Product() {Id = Guid.NewGuid(), Name = "Cheesecake", Description = "", Price = 29, ImageUrl = "#"},
//            new Product() {Id = Guid.NewGuid(), Name = "Muffin", Description = "", Price = 19, ImageUrl = "#"}
//        };

//        public Task<List<Product>> GetAllProducts()
//        {
//            return Task.FromResult(products);
//        }
//        public Task<Product> GetProduct(Guid Id)
//        {
//            Product product = products.SingleOrDefault(e => e.Id == Id);

//            return Task.FromResult(product);
//        }
//        public Task<Product> AddProduct(Product product)
//        {
//            return Task.FromResult(product);
//        }
//        public Task<Product> DeleteProduct(Guid Id)
//        {
//            var deleteProduct = products.SingleOrDefault(e => e.Id == Id);

//            products.Remove(deleteProduct);

//            return Task.FromResult(deleteProduct);
//        }

//        public Task<Product> UpdateProduct(Product product)
//        {
//            var productToUpdate = products.SingleOrDefault(p => p.Id == product.Id);
//            if (productToUpdate != null)
//            {
//                productToUpdate = product;
//            }
//            return Task.FromResult(productToUpdate);
//        }
//    }
//}