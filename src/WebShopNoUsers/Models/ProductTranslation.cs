using System.ComponentModel.DataAnnotations;

namespace WebShopNoUsers.Models {
    public class ProductTranslation {

        //Here we store properties changing with language
        public int ProductId { get; set; }
        public string Language { get; set; }
        [Required( ErrorMessage = "This field is required" )]
        public string ProductName { get; set; }
        [Required( ErrorMessage = "This field is required" )]
        public string ProductDescription { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public float ProductPrice { get; set; }
    }
}