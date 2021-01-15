using DeliCode.Library.Models;
using DeliCode.Web.Models;
using DeliCode.Web.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DeliCode.Web.Tests")]

namespace DeliCode.Web.Tests
{
    public class MockProductRepository : IProductRepository
    {
        internal List<Product> products;
        public MockProductRepository()
        {
            products = new List<Product>
            {
                new Product() { Id = new Guid("BD8F361A-E5E3-4F33-82CF-2594368D78A9"), Name = "Kanelbulle", Description = "Varma", Price = 9.50m, ImageUrl = "https://picsum.photos/id/133/286/180", AmountInStorage = 3 },
                new Product() { Id = Guid.NewGuid(), Name = "Kladdkarta", Description = "Extra florsocker", Price = 50.00m, ImageUrl = "https://picsum.photos/id/106/286/180", AmountInStorage = 0 },
                new Product() { Id = Guid.NewGuid(), Name = "Tårta", Description = "Innehåller grädde", Price = 79.90m, ImageUrl = "https://picsum.photos/id/292/286/180", AmountInStorage = 3 },
                new Product() { Id = Guid.NewGuid(), Name = "Cheesecake", Description = "En vanlig cheesecake", Price = 29.90m, ImageUrl = "https://picsum.photos/id/104/286/180", AmountInStorage = 1 },
                new Product() { Id = Guid.NewGuid(), Name = "Muffin", Description = "Stora", Price = 19.90m, ImageUrl = "https://picsum.photos/id/143/286/180", AmountInStorage = 4 }
            };

        }

        public Task<Product> Add(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<Product> Get(Guid id)
        {
            var product = products.SingleOrDefault(o => o.Id == id);
            return Task.FromResult(product);
        }

        public Task<List<Product>> GetAll()
        {
            return Task.FromResult(products);
        }

        public Task<Product> Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> Update(Product product)
        {
            var productToUpdate = products.SingleOrDefault(o => o.Id==product.Id);
            if (productToUpdate != null)
            {
                productToUpdate = product;
            }
            return Task.FromResult(productToUpdate);
        }
    }
}
