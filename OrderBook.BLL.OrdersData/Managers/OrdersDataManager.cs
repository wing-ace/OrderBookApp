using System;
using OrderBook.BLL.Common;
using OrderBook.BLL.Common.ResultModels;
using OrderBook.BLL.OrdersData.Services.Interfaces;
using OrderBook.DataContracts.Orders.Models;

namespace OrderBook.BLL.OrdersData.Managers
{
    /// <summary>
    /// The manager for get summary orders data
    /// </summary>
    public class OrdersDataManager : BaseManager
    {
        private readonly IOrdersDataService _ordersDataService;

        public OrdersDataManager(IOrdersDataService ordersDataService)
        {
            _ordersDataService = ordersDataService;
        }

        /// <summary>
        /// Get the summary sell order's data
        /// </summary>
        /// <param name="depthValue">The current depth value</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The number of items per one page</param>
        /// <returns>Manager result with paged list of sell orders</returns>
        public ManagerResult<SummaryOrdersDataInfoDto> GetSellOrdersSummaryData(decimal depthValue, int pageNumber,
            int pageSize)
        {
            try
            {
                return Ok(_ordersDataService.GetSellOrdersSummaryData(depthValue, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Error<SummaryOrdersDataInfoDto>(ex.Message);
            }
        }

        /// <summary>
        /// Get the summary buy order's data
        /// </summary>
        /// <param name="depthValue">The current depth value</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The number of items per one page</param>
        /// <returns>Manager result with paged list of buy orders</returns>
        public ManagerResult<SummaryOrdersDataInfoDto> GetBuyOrdersSummaryData(decimal depthValue, int pageNumber,
            int pageSize)
        {
            try
            {
                return Ok(_ordersDataService.GetBuyOrdersSummaryData(depthValue, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Error<SummaryOrdersDataInfoDto>(ex.Message);
            }
        }
    }
}
