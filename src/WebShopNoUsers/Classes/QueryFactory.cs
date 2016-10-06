using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebShopNoUsers.Models;
using WebShopNoUsers.ViewModels;

namespace WebShopNoUsers.Classes
{
    public class QueryFactory
    {
        private WebShopRepository _context;

        public QueryFactory(IWebShopRepository context) {
            _context = context as WebShopRepository;
        }

        public IQueryable<ProductViewModel> GetAllProducts() {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on
                        new { p.ProductId, Second = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName }
                        equals new { pt.ProductId, Second = pt.Language }
                        select new ProductViewModel {
                            ProductId = p.ProductId,
                            ProductName = pt.ProductName,
                            ProductDescription = pt.ProductDescription,
                            ProductCategoryId = p.ProductCategoryId,
                            ProductCategory = p.ProductCategory,
                            ProductPrice = pt.ProductPrice,
                            Language = pt.Language
                        };
            return query;
        }

        public IQueryable<ProductViewModel> GetProduct( int? id ) {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on
                        new { p.ProductId, Second = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName }
                        equals new { pt.ProductId, Second = pt.Language }
                        where p.ProductId == id
                        select new ProductViewModel {
                            ProductId = p.ProductId,
                            ProductName = pt.ProductName,
                            ProductDescription = pt.ProductDescription,
                            ProductCategoryId = p.ProductCategoryId,
                            ProductCategory = p.ProductCategory,
                            ProductPrice = pt.ProductPrice,
                            Language = pt.Language
                            
                        };
            return query;
        }

        public ProductTranslation GetTranslationForProduct(int id ) {
            var query = from pt in _context.ProductTranslations
                        where pt.ProductId == id && pt.Language == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName
                        select pt;
            return query.SingleOrDefault();
        }

        public IQueryable<string> GetTranslationsForProduct( int? id ) {
            var query = from pt in _context.ProductTranslations
                        where pt.ProductId == id
                        select pt.Language;
            return query;
        }
    }
}
