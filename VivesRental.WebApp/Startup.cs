using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VivesRental.Repository;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;
using VivesRental.Services;
using VivesRental.Services.Contracts;

namespace VivesRental.WebApp
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

            var connString = Configuration["ConnectionStrings:DatabaseContext"];

            //Register EF DbContext
            services.AddDbContext<VivesRentalDbContext>(options =>
                options.UseSqlServer(connString));
            services.AddScoped<IVivesRentalDbContext, VivesRentalDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Register Repositories
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderLineRepository, OrderLineRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            //Register Services
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IOrderLineService, OrderLineService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
