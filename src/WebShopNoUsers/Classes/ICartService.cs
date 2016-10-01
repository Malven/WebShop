using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopNoUsers.Models;

namespace WebShopNoUsers.Classes
{
    public interface ICartService
    {
        Task<List<Cart>> GetCarts(string userId);
        string GetCartUserId( HttpContext context );
    }
}
