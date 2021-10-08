using System;
using System.Collections.Generic;
using System.Linq;
using OrderBook.BLL.OrdersData.Services.Interfaces;
using OrderBook.CommonTools.Extensions;
using OrderBook.DataContracts.Orders.Models;
using OrderBook.Services.FetchOrdersDataBackgroundService;

namespace OrderBook.BLL.OrdersData.Services
{
    /// <summary>
    /// The implementation of interface for get summary order's data
    /// </summary>
    internal class OrdersDataService : IOrdersDataService
    {
        private readonly FetchOrdersDataBackgroundService _fetchOrdersDataBackgroundService;

        public OrdersDataService(IServiceProvider serviceProvider)
        {
            _fetchOrdersDataBackgroundService = serviceProvider.GetHostedService<FetchOrdersDataBackgroundService>();
        }

        /// <summary>
        /// Get the summary sell order's data
        /// </summary>
        /// <param name="depthValue">The current depth value</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The number of items per one page</param>
        /// <returns>The paged list of sell orders</returns>
        public SummaryOrdersDataInfoDto GetSellOrdersSummaryData(decimal depthValue, int pageNumber, int pageSize)
        {
            var sellOrders = _fetchOrdersDataBackgroundService.GetSellOrdersData()
                .OrderBy(orderDataDc => orderDataDc.Price).AsQueryable();
            return CreateSummaryOrdersDataInfoDto(GetCalculatedAndFilteredOrdersDataList(sellOrders, depthValue),
                pageNumber, pageSize, sellOrders.Count());
        }

        /// <summary>
        /// Get the summary buy order's data
        /// </summary>
        /// <param name="depthValue">The current depth value</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The number of items per one page</param>
        /// <returns>The paged list of buy orders</returns>
        public SummaryOrdersDataInfoDto GetBuyOrdersSummaryData(decimal depthValue, int pageNumber, int pageSize)
        {
            var buyOrders = _fetchOrdersDataBackgroundService.GetBuyOrdersData()
                .OrderByDescending(orderDataDc => orderDataDc.Price).AsQueryable();
            return CreateSummaryOrdersDataInfoDto(GetCalculatedAndFilteredOrdersDataList(buyOrders, depthValue),
                pageNumber, pageSize, buyOrders.Count());
        }

        /// <summary>
        /// Returns the filtered list of orders with calculated cumulative volume
        /// </summary>
        /// <param name="ordersData">The list of orders</param>
        /// <param name="depthValue">The current depth value for filtering</param>
        /// <returns>The filtered list of orders with calculated cumulative volume</returns>
        private static IEnumerable<OrderInfoDc> GetCalculatedAndFilteredOrdersDataList(IEnumerable<OrderDataFromExternalApiDto> ordersData, decimal depthValue)
        {
            var cumulativeVolume = 0M;
            return (
                from orderData in ordersData
                select new OrderInfoDc
                {
                    Price = orderData.Price,
                    Volume = orderData.Volume,
                    CumulativeVolume = cumulativeVolume += orderData.Price * orderData.Volume
                }
            ).Where(orderData => depthValue == 0 || orderData.CumulativeVolume <= depthValue);
        }

        /// <summary>
        /// Create the summary data about orders with pagination data
        /// </summary>
        /// <param name="ordersDataList">The list of orders</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The current page size</param>
        /// <param name="totalItemsCount">The total items count before filtering</param>
        /// <returns>The summary data about orders with pagination data</returns>
        private static SummaryOrdersDataInfoDto CreateSummaryOrdersDataInfoDto(IEnumerable<OrderInfoDc> ordersDataList, int pageNumber, int pageSize, int totalItemsCount)
            => new SummaryOrdersDataInfoDto
            {
                OrdersList = ordersDataList.ToPagedList(pageNumber, pageSize),
                PageNumber = pageNumber,
                TotalItemsCount = totalItemsCount
            };
    }
}
