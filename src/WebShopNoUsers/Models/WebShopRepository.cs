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
        public DbSet<ProductTranslation> ProductTranslations { get; set; }

        protected override void OnModelCreating( ModelBuilder modelBuilder ) {
            modelBuilder.Entity<ProductTranslation>()
            .HasKey( c => new { c.ProductId, c.Language } );
        }
        public WebShopRepository( DbContextOptions<WebShopRepository> options ) : base(options) {

        }
    }
}
