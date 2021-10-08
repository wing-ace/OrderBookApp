using Microsoft.Extensions.DependencyInjection;
using OrderBook.BLL.OrdersData.Managers;
using OrderBook.BLL.OrdersData.Services;
using OrderBook.BLL.OrdersData.Services.Interfaces;

namespace OrderBook.BLL.OrdersData
{
    /// <summary>
    /// The class for register dependencies in <see cref="OrderBook.BLL.OrdersData"/> library
    /// </summary>
    public static class OrdersDataDependenciesRegistrator
    {
        /// <summary>
        /// Register dependencies
        /// </summary>
        /// <param name="services">Collection of application's services</param>
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<OrdersDataManager>();
            services.AddScoped<IOrdersDataService, OrdersDataService>();
        }
    }
}
