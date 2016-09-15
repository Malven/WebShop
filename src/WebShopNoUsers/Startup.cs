using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebShopNoUsers.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.Text.RegularExpressions;

namespace WebShopNoUsers
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = @"Server=(localdb)\mssqllocaldb;Database=WebShop.Products;Trusted_Connection=True;";
            services.AddDbContext<WebShopRepository>( options => options.UseSqlServer( connection ) );
            services.AddLocalization( options => options.ResourcesPath = "Resources" );
            services.AddMvc()
                .AddViewLocalization( Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix )
                .AddDataAnnotationsLocalization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRequestLocalization( BuildLocalizationOptions() );
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "culture",
                    template: "{language:regex(^[a-z]{{2}}(-[A-Z]{{2}})*$)}/{controller=Home}/{action=Index}/{id?}" );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private RequestLocalizationOptions BuildLocalizationOptions() {
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("sv-SE")
            };

            var options = new RequestLocalizationOptions {
                DefaultRequestCulture = new RequestCulture( "en-US" ),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            options.RequestCultureProviders.Insert( 0, new CustomRequestCultureProvider( context => {
                //Quick and dirty parsing of language from url path, which looks like "/deDE/home/Index"
                var parts = context.Request.Path.Value.Split( '/' );
                if( parts.Length < 3 ) {
                    return Task.FromResult<ProviderCultureResult>( null );
                }
                var hasCulture = Regex.IsMatch( parts[ 1 ], @"^[a-z]{2}(-[A-Z]{2})*$" );
                if( !hasCulture ) {
                    return Task.FromResult<ProviderCultureResult>( null );
                }
                var culture = parts[ 1 ];
                return Task.FromResult( new ProviderCultureResult( culture ) );
            } ) );

            return options;
        }
    }
}
