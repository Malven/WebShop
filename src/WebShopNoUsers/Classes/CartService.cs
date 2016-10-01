using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WebShopNoUsers.Models;

namespace WebShopNoUsers.Classes
{
    public class CartService : ICartService {
        private WebShopRepository _context;

        public CartService(WebShopRepository context) {
            _context = context;
        }

        public async Task<List<Cart>> GetCarts( string userId ) {
            return await _context.Carts.Where( c => c.CartUserId == userId ).ToListAsync();
        }

        public string GetCartUserId( HttpContext context ) {
            if( context.Request.Cookies[ "cart" ] != null ) {
                var value = context.Request.Cookies[ "cart" ];
                //Applicera en HtmlEncode för att inte riskera få in
                //tex javascriptkod till sidan.
                var safevalue = HtmlEncoder.Default.Encode( value );
                return safevalue;
            } else {
                //Första besöket eller inga cookies påslagna.
                Guid tempCartID = Guid.NewGuid();
                context.Response.Cookies.Append(
                "cart",
                tempCartID.ToString(),
                new CookieOptions() {
                    Path = "/",
                    HttpOnly = false,  //True == client js can't read.
                    Secure = false,     //True == only https
                    Expires = DateTime.Now.AddMonths( 6 )  //Cookien sparas i 6 månader framåt.  DateTime.Now.AddDays(-1) tar bort cookien.
                }
                );
                return tempCartID.ToString();
            }
        }
    }
}
