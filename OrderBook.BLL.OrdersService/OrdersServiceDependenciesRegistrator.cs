using Microsoft.Extensions.DependencyInjection;
using OrderBook.BLL.OrdersService.Interfaces;

namespace OrderBook.BLL.OrdersService
{
    /// <summary>
    /// The class for register dependencies in <see cref="BLL.OrdersService"/> library
    /// </summary>
    public static class OrdersServiceDependenciesRegistrator
    {
        /// <summary>
        /// Register dependencies
        /// </summary>
        /// <param name="services">Collection of application's services</param>
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IOrdersService, Services.OrdersService>();
        }
    }
}
