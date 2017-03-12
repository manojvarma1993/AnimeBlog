using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Microsoft.EntityFrameworkCore;
using BlogAnime1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BlogAnime1
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940

        private readonly IConfigurationRoot configuration;
        public Startup(IHostingEnvironment env)
        {

            configuration = new ConfigurationBuilder().
                                   AddEnvironmentVariables().
                                   AddJsonFile(env.ContentRootPath + "/config.json").
                                   AddJsonFile(env.ContentRootPath + "/config.development.json", true).
                                   Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddTransient<FeatureToggles>(x => new FeatureToggles
            {
                EnableDeveloperExceptions = configuration.GetValue<bool>("FeatureToggles:EnableDeveloperExceptions")
            });
            services.AddTransient<Specials>();
            services.AddDbContext<BlogDataContext>(options =>
           {
               var connectionString = configuration.GetConnectionString("BlogDataContext");
                options.UseSqlServer(connectionString);
            });
            services.AddDbContext<IdentityDataContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("IdentityDataContext");
                options.UseSqlServer(connectionString);
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDataContext>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, FeatureToggles features)
        {
            loggerFactory.AddConsole();

            if (features.EnableDeveloperExceptions)
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseFileServer();
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.Contains("invalid"))
                {

                    throw new Exception("Error");
                }
                await next();
            });
            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute("Default", "{controller=Blog}/{action=Index}/{id?}");
            });
            
        }
    }
}
