using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopNoUsers.Models;

namespace WebShopNoUsers.ViewModels
{
    public class ProductViewModel {
        public int ProductId { get; set; }
        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public float ProductPrice { get; set; }
    }
}
