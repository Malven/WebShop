using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebShopNoUsers.Models
{
    public class Product
    {
        //In this Class we only store language independent information.
        //Primary Key
        public int ProductId { get; set; }
        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        [Required]
        public virtual ICollection<ProductTranslation> Translations { get; set; }
    }
}
