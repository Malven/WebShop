using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopNoUsers.Models;

namespace WebShopNoUsers.ViewModels
{
    public class ShoppingCartViewModel
    {
        public WebShopRepository repo;

        public ShoppingCartViewModel(HttpContext _context) {
            repo = _context.RequestServices.GetService( typeof( WebShopRepository ) ) as WebShopRepository;
        }

        public List<Cart> Carts { get; set; }
        public decimal CartTotal { get; set; }
        public string CurrentLanguage { get; set; }
    }
}
