using DeliCode.ProductAPI.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeliCode.ProductAPI.Tests
{
    class TestClientProvider : IDisposable
    {


        public TestServer Server { get; private set; }
        public HttpClient Client { get; private set; }
        public TestClientProvider()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddUserSecrets("0c530537-0b11-42bd-94c4-4488d6bc21ca")
                .AddJsonFile("appsettings.json")
                .Build();

            WebHostBuilder webHostBuilder = new WebHostBuilder();
            webHostBuilder.UseStartup<Startup>();
            webHostBuilder.UseConfiguration(configuration);
            webHostBuilder.ConfigureTestServices(s => s.AddDbContext<ProductDbContext>(options => options.UseSqlServer(configuration["SqlConnection:ProductDB"])));

            //Make sure startup is referring to correct dependency
            Server = new TestServer(webHostBuilder);
            Client = Server.CreateClient();
        }

        public void Dispose()
        {
            Server?.Dispose();
            Client?.Dispose();
        }
    }
}
