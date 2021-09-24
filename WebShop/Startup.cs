using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop
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
            services.AddControllersWithViews();
            services.AddDbContext<WsDBContext>(opt => opt.UseInMemoryDatabase("WsContext"));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                var scope = app.ApplicationServices.CreateScope();
                var context = scope.ServiceProvider.GetService<WsDBContext>();

                AddTestData(context);

                //var context = app.ApplicationServices.GetService<WsDBContext>();  AddTestData(context);


            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "FrmNewProduct",
                    pattern: "{controller=Home}/{action=FrmNewProduct}/{id?}");
            });
        }
        private static void AddTestData(WsDBContext context)
        {
            for (int i = 0; i <= 4; i++)
            {
                var product = new ProductModel { ProductId = i + 1, Name = "product" + i, Description = "description of product" + i, Price = i + 10, Categories = "cat1,cat2" };
                context.Products.Add(product);
                context.SaveChanges();
            }

            var settings = new Settings { settingsKey = 0, UseInMemoryStorage = true };
            context.Settings.Add(settings);
            context.SaveChanges();
        }
        }
}
