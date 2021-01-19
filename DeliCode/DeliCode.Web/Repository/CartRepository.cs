using DeliCode.Web.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliCode.Web.Services;

namespace DeliCode.Web.Repository
{
    public class CartRepository : ICartRepository
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        public CartRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Cart> GetCart(Guid sessionId)
        {
            var cart = _httpContextAccessor.HttpContext.Session.Get<Cart>(sessionId.ToString());
            if(cart==null)
            {
                cart = new Cart { SessionId = sessionId };
                cart=await SaveCart(cart);
            }
            return await Task.FromResult(cart);
        }

        public Task<Guid> GetCartCookie(string cookieName)
        {
            var cookie = _httpContextAccessor.HttpContext.Request.Cookies[cookieName];
            _=Guid.TryParse(cookie, out var result);
            return Task.FromResult(result);
        }

        public Task<Cart> SaveCart(Cart cart)
        {
            _httpContextAccessor.HttpContext.Session.Set<Cart>(cart.SessionId.ToString(), cart);
            return Task.FromResult(cart);
        }

        public Task<Guid> SetCartCookie(Guid id, CookieOptions options, string cookieName)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, id.ToString(), options);
            return Task.FromResult(id);
        }
    }
}
