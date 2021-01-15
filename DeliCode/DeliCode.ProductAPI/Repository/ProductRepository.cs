using DeliCode.ProductAPI.Data;
using DeliCode.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {

        private readonly ProductDbContext _context;
        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }


        public async Task<Product> AddProduct(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            catch
            {
                product = null;
            }

            return product;
        }


        public async Task<Product> DeleteProduct(Guid Id)
        {
            var order = await _context.Products.FindAsync(Id);

            try
            {
                _context.Products.Remove(order);
                await _context.SaveChangesAsync();
            }
            catch
            {
                order = null;
            }

            return order;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProduct(Guid Id)
        {
            return await _context.Products.SingleOrDefaultAsync(o => o.Id == Id);
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            catch
            {
                product = null;
            }

            return product;
        }
    }
}
