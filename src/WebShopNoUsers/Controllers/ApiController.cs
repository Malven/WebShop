using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebShopNoUsers.Models;
using WebShopNoUsers.ViewModels;
using WebShopNoUsers.Classes;

namespace WebShopNoUsers.Controllers {
    [Route( "api/[controller]" )]
    public class ApiController : Controller {
        WebShopRepository repo;
        QueryFactory qf;

        public ApiController( IWebShopRepository _repo ) {
            repo = _repo as WebShopRepository;
            qf = new QueryFactory( repo );
        }

        [HttpGet]
        public IEnumerable<ProductViewModel> GetAllProducts() {
            return qf.GetAllProducts();
        }

        [HttpGet( "{id}", Name = "GetProduct" )]
        public IActionResult GetProduct( string id ) {
            var item = qf.GetProduct( int.Parse( id ) ).FirstOrDefault();
            if( item == null ) {
                return NotFound();
            }
            return new ObjectResult( item );
        }
    }
}
