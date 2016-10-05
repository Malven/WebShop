using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopNoUsers.Models;
using WebShopNoUsers.ViewModels;
using Xunit;

namespace WebShopNoUsers.Controllers {
    public class Class1 {
        [Fact]
        public void PassingTest() {
            Assert.Equal( 4, Add( 2, 2 ) );
        }

        [Fact]
        public void FailingTest() {
            Assert.Equal( 5, Add( 2, 3 ) );
        }

        [Fact]
        public void ProductControllerTest() {
            var options = new Microsoft.EntityFrameworkCore.DbContextOptions<WebShopRepository>();

            IWebShopRepository test = new WebShopRepository( options);
            //TODO: Add dbcontext etc etc
            ProductsController controllerUnderTest = new ProductsController(test);
            var result = controllerUnderTest.Details( 1 );
            var viewResult = Assert.IsType<IQueryable<ProductViewModel>>( result );
            Assert.Equal( 1, viewResult.FirstOrDefault().ProductId );
        }


        int Add( int x, int y ) {
            return x + y;
        }
    }
}
