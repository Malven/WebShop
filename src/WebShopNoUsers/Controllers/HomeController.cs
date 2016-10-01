using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using WebShopNoUsers.Classes;
using WebShopNoUsers.Models;
using Microsoft.EntityFrameworkCore;

namespace WebShopNoUsers.Controllers
{
    public class HomeController : Controller {
        private readonly WebShopRepository _context;
        private QueryFactory queryFactory;

        public HomeController( WebShopRepository context ) {
            _context = context;
            queryFactory = new QueryFactory( _context );
        }

        [HttpPost]
        public IActionResult SetLanguage( string culture, string returnUrl ) {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue( new RequestCulture( culture ) ),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears( 1 ) }
            );

            return LocalRedirect( returnUrl );
        }

        public async Task<IActionResult> Index()
        {
            var query = queryFactory.GetAllProducts();
            //var cart = ShoppingCart.GetCart( HttpContext );
            //cart.AddToCart( new Product { ProductId = 1002, ProductCategoryId = 1 } );
            //cart.AddToCart( new Product { ProductId = 1002, ProductCategoryId = 1 } );
            //cart.AddToCart( new Product { ProductId = 1002, ProductCategoryId = 1 } );
            //cart.AddToCart( new Product { ProductId = 1003, ProductCategoryId = 1 } );
            //cart.AddToCart( new Product { ProductId = 1004, ProductCategoryId = 1 } );
            //cart.RemoveFromCart( 1002 );
            //var cartItems = cart.GetCartItems();
            //cart.CreateOrder( new Order { FirstName = "Marcus", OrderDate = DateTime.Now } );
            //cart.EmptyCart();
            //cartItems = cart.GetCartItems();
            ViewData[ "culture" ] = CultureInfo.CurrentCulture.Name;
            return View( await query.ToListAsync() );
        }

        [HttpGet]
        public IActionResult AddToCart(int id ) {
            var cart = ShoppingCart.GetCart( HttpContext );
            Product product = _context.Products.FirstOrDefault( x => x.ProductId == id );
            cart.AddToCart(product );
            return RedirectToAction( "Index" );
        }

        [HttpGet]
        public IActionResult RemoveFromCart(int id ) {
            var cart = ShoppingCart.GetCart( HttpContext );
            cart.RemoveFromCart( id );
            return RedirectToAction( "Index" );
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
