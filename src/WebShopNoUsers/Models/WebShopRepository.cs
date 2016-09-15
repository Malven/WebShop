using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShopNoUsers.Models
{
    public class WebShopRepository : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        public WebShopRepository( DbContextOptions<WebShopRepository> options ) : base(options) {

        }
    }
}
