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
        private static QueryFactory _query;

        public QueryFactory() {
        }

        public static QueryFactory Query {
            get {
                if( _query == null )
                    _query = new QueryFactory();
                return _query;
            }
        }

        public IQueryable<ProductViewModel> GetAllProducts( WebShopRepository _context ) {
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
                            ProductPrice = pt.ProductPrice
                        };
            return query;
        }

        public IQueryable<ProductViewModel> GetProduct( WebShopRepository _context, int? id ) {
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
                            ProductPrice = pt.ProductPrice
                        };
            return query;
        }
    }
}
