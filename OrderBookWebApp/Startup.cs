using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderBook.CommonDependenciesRegistration;
using OrderBook.Services.FetchOrdersDataBackgroundService;

namespace OrderBookWebApp
{
    /// <summary>
    /// The class for configure application on startup
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configure the collection of application's services
        /// </summary>
        /// <param name="services">Collection of application's services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            CommonDependenciesRegistrator.Register(services);

            services.AddHostedService<FetchOrdersDataBackgroundService>();
        }

        /// <summary>
        /// Configure the application's environment
        /// </summary>
        /// <param name="app">The application's configuration mechanism</param>
        /// <param name="env">The application's environment where it is running in</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
            });
        }
    }
}
