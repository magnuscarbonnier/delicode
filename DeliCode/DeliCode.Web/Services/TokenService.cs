using DeliCode.Web.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DeliCode.Web.Services
{
    public class TokenService : ITokenService
    {
        private readonly OrderApiTokenOptions _orderApiOptions;
        private readonly ProductApiTokenOptions _productApiOptions;
        private Uri _baseAddress = new Uri("https://dev-5fnthzy6.eu.auth0.com/oauth/token");
        public TokenService(ProductApiTokenOptions productApiOptions, OrderApiTokenOptions orderApiOptions)
        {
            _productApiOptions = productApiOptions;
            _orderApiOptions = orderApiOptions;
        }

        public ApiToken GetOrderApiToken()
        {
            var serializedoptions = JsonConvert.SerializeObject(_orderApiOptions);
            var client = new RestClient(_baseAddress);
            var request = new RestRequest(Method.POST);

            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", serializedoptions, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<ApiToken>(response.Content);
        }

        public ApiToken GetProductApiToken()
        {
            var serializedoptions = JsonConvert.SerializeObject(_productApiOptions);
            var client = new RestClient(_baseAddress);
            var request = new RestRequest(Method.POST);

            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", serializedoptions, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<ApiToken>(response.Content);
        }
    }
}
