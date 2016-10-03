using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebShopNoUsers.Models;
using WebShopNoUsers.Classes;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace WebShopNoUsers.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly WebShopRepository _context;

        private string eid = "5160";
        private string sharedSecret = "tE94QeKzSdUVSGe";
        

        public ShoppingCartController( WebShopRepository context ) {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult AddToCart( int id ) {
            var cart = ShoppingCart.GetCart( HttpContext );
            Product product = _context.Products.FirstOrDefault( x => x.ProductId == id );
            cart.AddToCart( product );
            return RedirectToAction( "Index", "Home" );
        }

        [HttpGet]
        public IActionResult RemoveFromCart( int id ) {
            var cart = ShoppingCart.GetCart( HttpContext );
            cart.RemoveFromCart( id );
            return RedirectToAction( "Index", "Home" );
        }

        public async Task<IActionResult> Checkout() {
            WebShopRepository repo = HttpContext.RequestServices.GetService( typeof( WebShopRepository ) ) as WebShopRepository;
            var myCart = ShoppingCart.GetCart( HttpContext ).GetCartItems();
            var cartItems = new List<Dictionary<string, object>>();
            foreach( var item in myCart ) {
                item.Product = repo.Products.FirstOrDefault( x => x.ProductId == item.ItemId );
                item.Product.Translations = new List<ProductTranslation>();
                item.Product.Translations.Add( repo.ProductTranslations.FirstOrDefault( x => x.Language == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName ) );
                cartItems.Add( new Dictionary<string, object> {
                    {"reference", item.ItemId },
                    {"name", item.Product.Translations.FirstOrDefault().ProductName },
                    { "quantity", item.Count },
                    { "unit_price", item.Product.Translations.FirstOrDefault().ProductPrice },
                    { "discount_rate", 0 },
                    { "tax_rate", 2500 }
                } );
            }
            cartItems.Add( new Dictionary<string, object> {
                { "type", "shipping_fee" },
                { "reference", "SHIPPING" },
                { "name", "Shipping Fee" },
                { "quantity", 1 },
                { "unit_price", 4900 },
                { "tax_rate", 2500 }
            } );

            var cart = new Dictionary<string, object> { { "items", cartItems } };

            var data = new Dictionary<string, object> {
                { "cart", cart }
            };

            var merchant = new Dictionary<string, object>
                {
                    { "id", eid },
                    { "back_to_store_uri", "http://127.0.0.1/" },
                    { "terms_uri", "http://127.0.0.1/" },
                    {
                        "checkout_uri",
                        "https://example.com/checkout.aspx"
                    },
                    {
                        "confirmation_uri",
                        "https://example.com/thankyou.aspx" +
                        "?klarna_order_id={checkout.order.id}"
                    },
                    {
                        "push_uri",
                        "https://example.com/push.aspx" +
                        "?klarna_order_id={checkout.order.id}"
                    }
                };

            data.Add( "purchase_country", "SE" );
            data.Add( "purchase_currency", "SEK" );
            data.Add( "locale", "sv-se" );
            data.Add( "merchant", merchant );

            WebShopNoUsers.Classes.Klarna k = new Classes.Klarna();
            var temp = k.CreateOrder( JsonConvert.SerializeObject( data ) );

            //TODO httpclient mot klarna och f� tillbaka en order

            return View();
        }

        private string CreateAuthorization( string data ) {
            //base64(hex(sha256 (request_payload + shared_secret)))

            using( var algorithm = SHA256.Create() ) {
                var hash = algorithm.ComputeHash( Encoding.UTF8.GetBytes( data ) );
                var base64 = System.Convert.ToBase64String( hash );
                return base64;
            }
        }
    }
}