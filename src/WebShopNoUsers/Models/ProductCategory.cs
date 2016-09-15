using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebShopNoUsers.Models
{
    public class ProductCategory
    {
        //Primary Key
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
