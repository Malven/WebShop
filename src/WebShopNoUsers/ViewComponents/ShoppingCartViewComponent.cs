using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebShopNoUsers.Classes;
using WebShopNoUsers.Models;
using WebShopNoUsers.ViewModels;

namespace WebShopNoUsers.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private ICartService _service;

        public ShoppingCartViewComponent( ICartService service ) {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            string cartUserId = _service.GetCartUserId( HttpContext );
            decimal cartTotal = 0;
            ShoppingCartViewModel vm = new ShoppingCartViewModel(HttpContext);
            vm.CurrentLanguage = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            vm.Carts = await _service.GetCarts( cartUserId );
            foreach( var item in vm.Carts ) {
                item.Product = vm.repo.Products.FirstOrDefault( x => x.ProductId == item.ItemId );
                ProductTranslation pt = vm.repo.ProductTranslations.FirstOrDefault( x => x.ProductId == item.ItemId && x.Language == vm.CurrentLanguage);
                item.Product.Translations = new List<ProductTranslation>();
                item.Product.Translations.Add( pt );
                cartTotal += (decimal)(item.Count * item.Product.Translations.FirstOrDefault(x => x.Language == vm.CurrentLanguage).ProductPrice);
            }
            vm.CartTotal = cartTotal;
            return View( vm );
        }
    }
}
