using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contoso.MVC.Configuration;
using Contoso.MVC.WebServiceAccess;
using Contoso.MVC.WebServiceAccess.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/*Se agregan estos using*/
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Contoso.MVC.Models;

namespace Contoso.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            /*Se agrega lo siguiente*/
            services.AddDbContext<AppIdentityDbContext>(options
                =>options.
                UseSqlServer(Configuration["Data:ContosoIdentity:ConnectionString"])
                );
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().
                AddDefaultTokenProviders()
                ;
            /*Mi clase tipo IdentitiUser es AppUser*/
            /*******/

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IWebServiceLocator, WebServiceLocator>();
            /*Cuando Te llamen a IWebApiCalls manda a instanciar webApiCalls*/
            services.AddSingleton<IWebApCalls, WebApiCalls>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            /*Se agrega*/
            app.UseAuthentication();
            /*****************/
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
