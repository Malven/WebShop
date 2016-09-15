using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebShopNoUsers.Models
{
    public class Product
    {
        //Primary Key
        public int ProductId { get; set; }
        [Required( ErrorMessage = "This field is required" )]
        [Display( Name = "Product Name")]
        public string ProductName { get; set; }
        [Required( ErrorMessage = "Field is empty" )]
        [DataType( DataType.Currency )]
        public decimal Price { get; set; }
        [MaxLength( 250, ErrorMessage = "Får innehålla max 250 tecken" )]
        [MinLength( 10, ErrorMessage = "Får innehålla minst 10 tecken" )]
        public string Description { get; set; }

        //Foreign Key
        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
    }
}
