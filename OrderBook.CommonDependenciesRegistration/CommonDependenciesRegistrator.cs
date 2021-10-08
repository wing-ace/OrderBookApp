using Microsoft.Extensions.DependencyInjection;
using OrderBook.BLL.OrdersData;
using OrderBook.ExceptionsHandler;
using OrderBook.Kit.RestClientWrapper;

namespace OrderBook.CommonDependenciesRegistration
{
    /// <summary>
    /// The class for register common dependencies
    /// </summary>
    public static class CommonDependenciesRegistrator
    {
        /// <summary>
        /// Register dependencies
        /// </summary>
        /// <param name="services">Collection of application's services</param>
        public static void Register(IServiceCollection services)
        {
            ExceptionsHandlerDependenciesRegistrator.Register(services);
            OrdersDataDependenciesRegistrator.Register(services);
            RestClientWrapperDependenciesRegistrator.Register(services);
        }
    }
}
