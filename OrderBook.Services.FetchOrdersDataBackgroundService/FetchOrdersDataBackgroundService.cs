using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderBook.DataContracts.Orders.Models;
using OrderBook.Kit.RestClientWrapper.Interfaces;
using OrderBook.Services.RefreshOrdersDataBackgroudService.Constants;

namespace OrderBook.Services.FetchOrdersDataBackgroundService
{
    /// <summary>
    /// The background service for fetch orders data from external API
    /// </summary>
    public class FetchOrdersDataBackgroundService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FetchOrdersDataBackgroundService> _logger;

        private readonly ManualResetEvent _eventLocker = new ManualResetEvent(false);
        private readonly ConcurrentBag<OrderDataFromExternalApiDto> _sellOrdersConcurrentBag = new ConcurrentBag<OrderDataFromExternalApiDto>();
        private readonly ConcurrentBag<OrderDataFromExternalApiDto> _buyOrdersConcurrentBag = new ConcurrentBag<OrderDataFromExternalApiDto>();

        public FetchOrdersDataBackgroundService(ILogger<FetchOrdersDataBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// The async method for start background service execution
        /// </summary>
        /// <param name="cancellationToken">The token for stopping method execution</param>
        /// <returns>The result of starting service</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetch orders data background service running.");

            _timer = new Timer(FetchOrdersData, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
            return Task.CompletedTask;
        }

        /// <summary>
        /// The async method for stop background service execution
        /// </summary>
        /// <param name="cancellationToken">The token for stopping method execution</param>
        /// <returns>The result of stopping service</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetch orders data background service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Get the list of sell orders
        /// </summary>
        /// <returns>The list of sell orders</returns>
        public IEnumerable<OrderDataFromExternalApiDto> GetSellOrdersData()
        {
            _eventLocker.WaitOne();
            return _sellOrdersConcurrentBag.ToArray();
        }

        /// <summary>
        /// Get the list of buy orders
        /// </summary>
        /// <returns>The list of buy orders</returns>
        public IEnumerable<OrderDataFromExternalApiDto> GetBuyOrdersData()
        {
            _eventLocker.WaitOne();
            return _buyOrdersConcurrentBag.ToArray();
        }

        /// <summary>
        /// The main method for fetching data from external API
        /// and updating internal lists of orders
        /// </summary>
        /// <param name="state">The object containing application-specific information</param>
        private void FetchOrdersData(object state)
        {
            using var scope = _serviceProvider.CreateScope();
            try
            {
                var restClientWrapper = scope.ServiceProvider.GetRequiredService<IRestClientWrapper>();
                var ordersData =
                    restClientWrapper.ExecuteGetResquest<GetOrderBookFromExternalApiResponseDto>(FetchOrdersDataConstants.ExternalApiUri,
                        CreateQueryParams());

                if (ordersData == null)
                {
                    LogError("Cannot deserialize data from external API response.");
                    return;
                }

                _eventLocker.Reset();
                AddItemsToConcurentBag(ordersData.BuyOrdersData, _buyOrdersConcurrentBag);
                AddItemsToConcurentBag(ordersData.SellOrdersData, _sellOrdersConcurrentBag);
                _eventLocker.Set();
            }
            catch (Exception ex)
            {
                LogError(
                    $"An error has occurred while fetching orders data from the external API. Reason: {ex.Message}");
            }
        }

        /// <summary>
        /// Create the list of key value pairs for fetch data from external API
        /// </summary>
        /// <returns>The list of key value pairs</returns>
        private static List<KeyValuePair<string, object>> CreateQueryParams()
            => new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(FetchOrdersDataConstants.PrimaryCurrencyCodeParamName,
                    FetchOrdersDataConstants.PrimaryCurrencyCodeParamValue),
                new KeyValuePair<string, object>(FetchOrdersDataConstants.SecondaryCurrencyCodeParamName,
                    FetchOrdersDataConstants.SecondaryCurrencyCodeParamValue)
            };

        /// <summary>
        /// Write a error message with <see cref="ILogger"/>
        /// </summary>
        /// <param name="errorMessage">The message about error</param>
        private void LogError(string errorMessage)
            => _logger.LogError(errorMessage);

        /// <summary>
        /// Add items from <see cref="ordersData"/>
        /// to <see cref="_buyOrdersConcurrentBag"/> or <see cref="_sellOrdersConcurrentBag"/>
        /// </summary>
        /// <param name="ordersData">List of orders form external API</param>
        /// <param name="concurrentBag">
        /// <see cref="_buyOrdersConcurrentBag"/> or <see cref="_sellOrdersConcurrentBag"/>
        /// </param>
        private static void AddItemsToConcurentBag(List<OrderDataFromExternalApiDto> ordersData, ConcurrentBag<OrderDataFromExternalApiDto> concurrentBag)
        {
            concurrentBag.Clear();
            ordersData.ForEach(concurrentBag.Add);
        }

        /// <summary>
        /// Dispone <see cref="_timer"/>
        /// </summary>
        public void Dispose()
            => _timer?.Dispose();
    }
}
