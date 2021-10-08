using System;
using System.Collections.Generic;
using System.Linq;
using OrderBook.BLL.OrdersService.Interfaces;
using OrderBook.CommonTools.Extensions;
using OrderBook.DataContracts.Orders.Models;
using OrderBook.Services.RefreshOrdersDataBackgroudService;

namespace OrderBook.BLL.OrdersService.Services
{
    /// <summary>
    /// The implementation of interface for get summary order's data
    /// </summary>
    public class OrdersService : IOrdersService
    {
        private readonly FetchOrdersDataBackgroundService _fetchOrdersDataBackgroundService;

        public OrdersService(IServiceProvider serviceProvider)
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
            var sellOrders = _fetchOrdersDataBackgroundService.GetSellOrdersData();

            var sellOrdersData = sellOrders.OrderBy(dt1 => dt1.Price).ToList();
            var finalOrdersData = CalculateData(sellOrdersData).Where(dt => depthValue == 0 || dt.CumulativeVolume <= depthValue).ToList();

            return CreateSummaryOrdersDataInfoDto(finalOrdersData, pageNumber, pageSize, finalOrdersData.Count);
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
            var buyOrders = _fetchOrdersDataBackgroundService.GetBuyOrdersData();

            var buyOrdersData = buyOrders.OrderByDescending(dt1 => dt1.Price).ToList();
            var finalOrdersData = CalculateData(buyOrdersData).Where(dt => depthValue == 0 || dt.CumulativeVolume <= depthValue).ToList();

            return CreateSummaryOrdersDataInfoDto(finalOrdersData, pageNumber, pageSize, finalOrdersData.Count);
        }

        private List<OrderInfoDc> CalculateData(List<OrderDataDc> orderDataList)
        {
            var calculatedData = new List<OrderInfoDc>();
            var cumulativeVolume = 0M;
            orderDataList.ForEach(dt1 =>
            {
                cumulativeVolume += dt1.Price * dt1.Volume;
                var orderInfoDto = new OrderInfoDc
                {
                    Price = dt1.Price,
                    Volume = dt1.Volume,
                    CumulativeVolume = cumulativeVolume
                };
                calculatedData.Add(orderInfoDto);
            });

            return calculatedData;
        }

        private static SummaryOrdersDataInfoDto CreateSummaryOrdersDataInfoDto(IEnumerable<OrderInfoDc> ordersDataList, int pageNumber, int pageSize, int totalItemsCount)
            => new SummaryOrdersDataInfoDto
            {
                OrdersList = GetPagedList(ordersDataList, pageNumber, pageSize),
                PageNumber = pageNumber,
                TotalItemsCount = totalItemsCount
            };

        private static List<TModel> GetPagedList<TModel>(IEnumerable<TModel> dataList, int pageNumber, int pageSize)
        {
            var itemsToSkip = (pageNumber - 1) * pageSize;
            return dataList.Skip(itemsToSkip).Take(pageSize).ToList();
        }
    }
}
