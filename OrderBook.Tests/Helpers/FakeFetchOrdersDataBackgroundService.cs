using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using OrderBook.DataContracts.Orders.Enums;
using OrderBook.DataContracts.Orders.Models;
using OrderBook.Services.FetchOrdersDataBackgroundService.Interfaces;

namespace OrderBook.Tests.Helpers
{
    /// <summary>
    /// The fake implementation of background service for fetch orders data
    /// </summary>
    public class FakeFetchOrdersDataBackgroundService : IFetchOrdersDataBackgroundService, IHostedService
    {
        private readonly List<OrderDataFromExternalApiDto> _sellOrdersList = new List<OrderDataFromExternalApiDto>();
        private readonly List<OrderDataFromExternalApiDto> _buyOrdersList = new List<OrderDataFromExternalApiDto>();

        /// <summary>
        /// <inheritdoc cref="IFetchOrdersDataBackgroundService.GetSellOrdersData"/>
        /// </summary>
        public IEnumerable<OrderDataFromExternalApiDto> GetSellOrdersData()
            => _sellOrdersList.ToArray();

        /// <summary>
        /// <inheritdoc cref="IFetchOrdersDataBackgroundService.GetBuyOrdersData"/>
        /// </summary>
        public IEnumerable<OrderDataFromExternalApiDto> GetBuyOrdersData()
            => _buyOrdersList.ToArray();

        /// <summary>
        /// Set the test orders data to internal list of orders
        /// </summary>
        /// <param name="orderType"><see cref="OrderTypeEnum"/></param>
        /// <param name="ordersList">The set of orders</param>
        public void PrepareTestData(OrderTypeEnum orderType, List<OrderDataFromExternalApiDto> ordersList)
        {
            if (orderType == OrderTypeEnum.SellOrder)
            {
                UpdateOrdersTestData(_sellOrdersList, ordersList);
                return;
            }

            UpdateOrdersTestData(_buyOrdersList, ordersList);
        }

        /// <summary>
        /// Update orders data in internal list with new test data
        /// </summary>
        /// <param name="internalOrdersList"><see cref="_buyOrdersList"/> or <see cref="_sellOrdersList"/></param>
        /// <param name="newOrdersData">The new set of orders</param>
        private void UpdateOrdersTestData(List<OrderDataFromExternalApiDto> internalOrdersList, List<OrderDataFromExternalApiDto> newOrdersData)
        {
            internalOrdersList.Clear();
            internalOrdersList.AddRange(newOrdersData);
        }

        public Task StartAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
