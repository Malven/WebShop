using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
            var carts = await _service.GetCarts( cartUserId );
            ShoppingCartViewModel vm = new ShoppingCartViewModel(HttpContext);
            vm.Carts = carts;
            foreach( var item in vm.Carts ) {
                item.Product = vm.repo.Products.FirstOrDefault( x => x.ProductId == item.ItemId );
                List<ProductTranslation> pts = vm.repo.ProductTranslations.Where( x => x.ProductId == item.ItemId ).ToList();
                item.Product.Translations = new List<ProductTranslation>();
                foreach( var pt in pts ) {
                    item.Product.Translations.Add( pt );
                }
            }
            return View( vm );
        }
    }
}
