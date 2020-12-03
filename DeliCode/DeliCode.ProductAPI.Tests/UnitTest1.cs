using DeliCode.Library.Models;
using DeliCode.ProductAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;



namespace DeliCode.ProductAPI.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void GetAllProductsShouldReturnListOfProducts()
        {
            ProductRepository repos = new ProductRepository();
            var expected = repos.Products;
            List<Product> actual = repos.GetAllProducts();
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void GetProductShouldReturnSingleProduct()
        {
            ProductRepository repos = new ProductRepository();
            var expected = repos.Products
                .Where(e => e.Id == new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"))
                .SingleOrDefault(); ;
            Product product = repos.GetProduct(new Guid("11223344-5566-7788-99AA-BBCCDDEEFF00"));
            Assert.Equal(expected, product);
        }
        //Tests:

        // PUT: api/Products/5
        // PutProduct(Guid id, Product product)


        // POST: api/Products
        // PostProduct(Product product)


        // DELETE: api/Products/5
        // DeleteProduct(Guid id)


        // ProductExists(Guid id)
    }
}
