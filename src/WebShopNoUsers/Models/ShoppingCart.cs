using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace WebShopNoUsers.Models
{
    public partial class ShoppingCart
    {
        WebShopRepository repo;
        string shoppingCartUserId { get; set; }

        public ShoppingCart(HttpContext context) {
            repo = context.RequestServices.GetService( typeof( WebShopRepository ) ) as WebShopRepository;
        }

        public static ShoppingCart GetCart(HttpContext context ) {
            var cart = new ShoppingCart( context );
            cart.shoppingCartUserId = cart.GetCartId( context );
            return cart;
        }

        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart( Controller controller ) {
            return GetCart( controller.HttpContext );
        }

        public void AddToCart(Product product ) {
            var cartItem = repo.Carts.SingleOrDefault( c => c.CartUserId == shoppingCartUserId && c.ItemId == product.ProductId );

            if(cartItem == null ) {
                //Skapa en ny cart för det finns ingen för denna användaren
                cartItem = new Cart {
                    ItemId = product.ProductId,
                    CartUserId = shoppingCartUserId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                repo.Carts.Add( cartItem );
            }
            else {
                //Om produkten finns i en befintlig cart
                cartItem.Count++;
            }
            //Spara
            repo.SaveChanges();
        }

        public int RemoveFromCart(int id ) {
            var cartItem = repo.Carts.Single( cart => cart.CartUserId == shoppingCartUserId && cart.ItemId == id );

            int itemCount = 0;
            if(cartItem != null ) {
                if(cartItem.Count > 1 ) {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else {
                    repo.Carts.Remove( cartItem );
                }
                repo.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart() {
            var cartItems = repo.Carts.Where( c => c.CartUserId == shoppingCartUserId );
            foreach( var cartItem in cartItems ) {
                repo.Carts.Remove( cartItem );
            }
            repo.SaveChanges();
        }

        public List<Cart> GetCartItems() {
            return repo.Carts.Where( c => c.CartUserId == shoppingCartUserId ).ToList();
        }

        public int GetCount() {
            int? count = ( from cartItems in repo.Carts where cartItems.CartUserId == shoppingCartUserId select (int?)cartItems.Count ).Sum();
            return count ?? 0;
        }

        public float GetTotal() {
            float? total = ( from cartItems in repo.Carts
                               where cartItems.CartUserId == shoppingCartUserId
                               select (float?)cartItems.Count * cartItems.Product.Translations.First().ProductPrice ).Sum();
            return total ?? 0;
        }



        public string GetCartId( HttpContext context ) {
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
