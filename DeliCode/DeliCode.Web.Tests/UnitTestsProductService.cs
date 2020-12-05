using DeliCode.Library.Models;
using DeliCode.Web.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeliCode.Web.Tests
{
    public class UnitTestsProductService
    {
        private readonly ProductService _productService;
        public UnitTestsProductService()
        {
            _productService = new ProductService(new HttpClient());
        }

        //GetAll()
        [Fact]
        public void GetAllProductsShouldReturnListOfProducts()
        {
            //Arrange
            var expected = 5;

            //Act
            var actual = _productService.GetAll();

            //Assert
            Assert.Equal(expected, actual.Result.Count);
        }
        //GetById()

        //Insert
        
        //Update
        
        //Delete
    }
}
