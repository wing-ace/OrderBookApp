using System;
using System.Collections.Generic;
using System.Linq;
using OrderBook.BLL.OrdersData.Services.Interfaces;
using OrderBook.CommonTools.Extensions;
using OrderBook.DataContracts.Orders.Enums;
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

        private readonly IDictionary<OrderTypeEnum, Func<IEnumerable<OrderDataFromExternalApiDto>>>
            _mapOrderTypeToGetOrdersMethodDictionary;

        public OrdersDataService(IServiceProvider serviceProvider)
        {
            _fetchOrdersDataBackgroundService = serviceProvider.GetHostedService<FetchOrdersDataBackgroundService>();
            _mapOrderTypeToGetOrdersMethodDictionary = CreateMapOrderTypeToGetOrdersMethodDictionary();
        }

        /// <summary>
        /// Get the summary orders data
        /// </summary>
        /// <param name="orderType">The type of order (<see cref="OrderTypeEnum"/>)</param>
        /// <param name="depthValue">The current depth value</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The number of items per one page</param>
        /// <returns>The paged list of buy orders</returns>
        public SummaryOrdersDataInfoDto GetOrdersSummaryDataByType(OrderTypeEnum orderType, decimal depthValue,
            int pageNumber,
            int pageSize)
        {
            if (!_mapOrderTypeToGetOrdersMethodDictionary.ContainsKey(orderType))
                throw new InvalidOperationException(
                    $"Cannot determine select orders method for order type {orderType}");

            var ordersData = _mapOrderTypeToGetOrdersMethodDictionary[orderType]();
            var calculatedAndFilteredOrdersData =
                GetCalculatedAndFilteredOrdersDataList(ordersData, depthValue).ToList();
            return CreateSummaryOrdersDataInfoDto(calculatedAndFilteredOrdersData,
                pageNumber, pageSize, calculatedAndFilteredOrdersData.Count);
        }

        /// <summary>
        /// Returns the filtered list of orders with calculated cumulative volume
        /// </summary>
        /// <param name="ordersData">The list of orders</param>
        /// <param name="depthValue">The current depth value for filtering</param>
        /// <returns>The filtered list of orders with calculated cumulative volume</returns>
        private static IEnumerable<OrderInfoDc> GetCalculatedAndFilteredOrdersDataList(
            IEnumerable<OrderDataFromExternalApiDto> ordersData, decimal depthValue)
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
        private static SummaryOrdersDataInfoDto CreateSummaryOrdersDataInfoDto(List<OrderInfoDc> ordersDataList,
            int pageNumber, int pageSize, int totalItemsCount)
            => new SummaryOrdersDataInfoDto
            {
                OrdersList = ordersDataList.ToPagedList(pageNumber, pageSize),
                PageNumber = pageNumber,
                TotalItemsCount = totalItemsCount
            };

        /// <summary>
        /// Create the dictionary for
        /// map inputed order type to select orders data list
        /// </summary>
        /// <returns>
        /// The dictionary for map inputed order type to select orders data list
        /// </returns>
        private Dictionary<OrderTypeEnum, Func<IEnumerable<OrderDataFromExternalApiDto>>>
            CreateMapOrderTypeToGetOrdersMethodDictionary()
            => new Dictionary<OrderTypeEnum, Func<IEnumerable<OrderDataFromExternalApiDto>>>
            {
                {
                    OrderTypeEnum.BuyOrder, () =>
                    {
                        return _fetchOrdersDataBackgroundService.GetBuyOrdersData()
                            .OrderByDescending(orderDataDc => orderDataDc.Price);
                    }
                },
                {
                    OrderTypeEnum.SellOrder, () =>
                    {
                        return _fetchOrdersDataBackgroundService.GetSellOrdersData()
                            .OrderBy(orderDataDc => orderDataDc.Price);
                    }
                }
            };
    }
}