using Microsoft.Extensions.DependencyInjection;
using OrderBook.ExceptionsHandler.Interfaces;

namespace OrderBook.ExceptionsHandler
{
    /// <summary>
    /// The class for register dependencies in <see cref="OrderBook.ExceptionsHandler"/> library
    /// </summary>
    public static class ExceptionsHandlerDependenciesRegistrator
    {
        /// <summary>
        /// Register dependencies
        /// </summary>
        /// <param name="services">Collection of application's services</param>
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IExceptionsHandler, ExceptionsHandler>();
        }
    }
}
