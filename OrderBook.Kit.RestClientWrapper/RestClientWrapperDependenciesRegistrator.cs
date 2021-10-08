using Microsoft.Extensions.DependencyInjection;
using OrderBook.Kit.RestClientWrapper.Interfaces;

namespace OrderBook.Kit.RestClientWrapper
{
    /// <summary>
    /// The class for register dependencies in <see cref="Kit.RestClientWrapper"/> library
    /// </summary>
    public static class RestClientWrapperDependenciesRegistrator
    {
        /// <summary>
        /// Register dependencies
        /// </summary>
        /// <param name="services">Collection of application's services</param>
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IRestClientWrapper, RestClientWrapper>();
        }
    }
}
