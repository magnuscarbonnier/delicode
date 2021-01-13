using DeliCode.Web.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.Web.Repository
{
    public interface ICartRepository
    {
        Task<Cart> GetCart(Guid sessionId);
        Task<Cart> SaveCart(Cart cart);
        Task<Guid> GetCartCookie(string cookieName);
        Task<Guid> SetCartCookie(Guid id, CookieOptions options, string cookieName);
    }
}
