using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OrderBook.CommonDependenciesRegistration;
using OrderBook.Services.FetchOrdersDataBackgroundService.Interfaces;

namespace OrderBook.Tests.Helpers
{
    /// <summary>
    /// The fixture for tests environment
    /// </summary>
    public class CommonTestsFixture
    {
        public CommonTestsFixture()
        {
            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddSingleton<IFetchOrdersDataBackgroundService, FakeFetchOrdersDataBackgroundService>();
            serviceCollection.AddHostedService(serviceProvider =>
                (FakeFetchOrdersDataBackgroundService)serviceProvider.GetService<IFetchOrdersDataBackgroundService>());

            serviceCollection.AddLogging(builder => builder.AddProvider(NullLoggerProvider.Instance));

            CommonDependenciesRegistrator.Register(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public ServiceProvider ServiceProvider { get; }
    }
}
