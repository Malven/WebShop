using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace WebShopNoUsers.Controllers
{
    public class HomeController : Controller
    {

        [HttpPost]
        public IActionResult SetLanguage( string culture, string returnUrl ) {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue( new RequestCulture( culture ) ),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears( 1 ) }
            );

            return LocalRedirect( returnUrl );
        }

        public IActionResult Index()
        {
            ViewData[ "culture" ] = CultureInfo.CurrentCulture.Name;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
