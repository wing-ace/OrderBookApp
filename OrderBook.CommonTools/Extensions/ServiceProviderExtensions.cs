using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OrderBook.CommonTools.Extensions
{
    /// <summary>
    /// The extensions for <see cref="IServiceProvider"/>
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Get background service with base type of <see cref="IHostedService"/>
        /// </summary>
        /// <typeparam name="TBackgroundServiceType">Type of background service for getting</typeparam>
        /// <param name="serviceProvider">The class that contains application's service collection</param>
        /// <returns>The background service instance of type <see cref="TBackgroundServiceType"/></returns>
        public static TBackgroundServiceType GetHostedService<TBackgroundServiceType>
            (this IServiceProvider serviceProvider) =>
            serviceProvider
                .GetServices<IHostedService>()
                .OfType<TBackgroundServiceType>()
                .FirstOrDefault();
    }
}
