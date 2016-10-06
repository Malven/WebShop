using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopNoUsers.Models;
using WebShopNoUsers.ViewModels;
using Xunit;

namespace WebShopNoUsers.Controllers {
    public class Class1 {

        private static DbContextOptions<WebShopRepository> CreateNewContextOptions() {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<WebShopRepository>();
            builder.UseInMemoryDatabase()
                   .UseInternalServiceProvider( serviceProvider );

            return builder.Options;
        }

        [Fact]
        public async Task DetailProduct() {
            //Arrange
            // All contexts that share the same service provider will share the same InMemory database
            var options = CreateNewContextOptions();
            CreateContextDb( options );

            // Use a clean instance of the context to run the test
            using( var context = new WebShopRepository( options ) ) {
                var service = new ProductsController( context );

                //Act
                var result = await service.Details( 1 );
                //Assert
                var viewResult = Assert.IsType<ViewResult>( result );
                var model = Assert.IsAssignableFrom<ProductViewModel>(
                    viewResult.ViewData.Model );
                Assert.Equal( 1, model.ProductId );
            }
        }

        [Fact]
        public async Task EditProduct() {
            //Arrange
            // All contexts that share the same service provider will share the same InMemory database
            var options = CreateNewContextOptions();
            CreateContextDb( options );

            // Use a clean instance of the context to run the test
            using( var context = new WebShopRepository( options ) ) {
                var service = new ProductsController( context );
                //Act
                var result = await service.Edit( 1 );
                //Assert
                var viewResult = Assert.IsType<ViewResult>( result );
                var model = Assert.IsAssignableFrom<ProductViewModel>(
                    viewResult.ViewData.Model );
                Assert.Equal( 1, model.ProductId );
            }
        }

        private static void CreateContextDb( DbContextOptions<WebShopRepository> options ) {
            // Insert seed data into the database using one instance of the context
            using( var context = new WebShopRepository( options ) ) {
                context.ProductCategories.Add( new ProductCategory { ProductCategoryId = 1, ProductCategoryName = "Kläder", Products = new List<Product>() } );
                context.Products.Add( new Product { ProductId = 1, ProductCategoryId = 1, Translations = new List<ProductTranslation>(), ProductCategory = new ProductCategory() } );
                context.Products.Add( new Product { ProductId = 2, ProductCategoryId = 1, Translations = new List<ProductTranslation>(), ProductCategory = new ProductCategory() } );
                context.ProductTranslations.Add( new ProductTranslation { ProductId = 1, Language = "en", ProductName = "test" } );
                context.ProductTranslations.Add( new ProductTranslation { ProductId = 2, Language = "en", ProductName = "test" } );
                context.ProductTranslations.Add( new ProductTranslation { ProductId = 1, Language = "sv", ProductName = "test" } );
                context.ProductTranslations.Add( new ProductTranslation { ProductId = 2, Language = "sv", ProductName = "test" } );
                context.SaveChanges();
            }
        }
    }
}
