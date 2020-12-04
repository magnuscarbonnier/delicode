using DeliCode.Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DeliCode.Web.Services
{
    public interface IJwtTokenService
    {
        Task<string> GenerateJwtToken(MicroserviceType microserviceType);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GenerateJwtToken(MicroserviceType microserviceType)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Subject = new ClaimsIdentity(
                    new Claim[] {
                        new Claim(ClaimTypes.Sid, microserviceType.ToString())
                    }),
                Expires = DateTime.UtcNow.AddMinutes(2),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return await Task.FromResult(tokenHandler.WriteToken(token));
        }
    }
}
