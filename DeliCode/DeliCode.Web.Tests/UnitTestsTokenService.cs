using DeliCode.Library.Models;
using DeliCode.Web.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace DeliCode.Web.Tests
{

    public class UnitTestsTokenService
    {
        private IConfiguration _config;
        
        public UnitTestsTokenService()
        {
            //use appsettings and secrets from DeliCode.Web
            _config = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .AddUserSecrets("0c530537-0b11-42bd-94c4-4488d6bc21ca")
        .Build();
        }

        [Fact]
        async Task GenerateJwtToken_OfTypeOrderApi_ShouldReturnJwtTokenClaimOrderApi()
        {
            //Arrange
            JwtTokenService tokenService = new JwtTokenService(_config);
            MicroserviceType claim = MicroserviceType.OrderApi;
            string validClaimResult = claim.ToString();

            //Act
            var tokenResult = await tokenService.GenerateJwtToken(claim);
            string claimResult = await GetJwtClaim(tokenResult, ClaimTypes.Sid);
            
            //Assert
            Assert.Equal(validClaimResult, claimResult);
        }

        [Fact]
        async Task GenerateJwtToken_OfTypeProductApi_ShouldReturnJwtTokenClaimProductApi()
        {
            //Arrange
            JwtTokenService tokenService = new JwtTokenService(_config);
            MicroserviceType claim = MicroserviceType.ProductApi;
            string validClaimResult = claim.ToString();

            //Act
            var tokenResult = await tokenService.GenerateJwtToken(claim);
            string claimResult = await GetJwtClaim(tokenResult, ClaimTypes.Sid);

            //Assert
            Assert.Equal(validClaimResult, claimResult);
        }

        //Get the first value of specific claimtype in token, if any, else throws exception
        private async Task<string> GetJwtClaim(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            var claim = securityToken.Claims.First(claim=>claim.Type==claimType).Value;
            return await Task.FromResult(claim);
        }
    }
}
