using System;
using OrderBook.BLL.Common;
using OrderBook.BLL.Common.ResultModels;
using OrderBook.BLL.OrdersData.Services.Interfaces;
using OrderBook.DataContracts.Orders.Enums;
using OrderBook.DataContracts.Orders.Models;
using OrderBook.ExceptionsHandler.Interfaces;

namespace OrderBook.BLL.OrdersData.Managers
{
    /// <summary>
    /// The manager for get summary orders data
    /// </summary>
    public class OrdersDataManager : BaseManager
    {
        private readonly IOrdersDataService _ordersDataService;

        public OrdersDataManager(IOrdersDataService ordersDataService, IExceptionsHandler exceptionsHandler) : base(exceptionsHandler)
        {
            _ordersDataService = ordersDataService;
        }

        /// <summary>
        /// Get the orders summary data based on order type
        /// </summary>
        /// <param name="orderType">The type of order (<see cref="OrderTypeEnum"/>)</param>
        /// <param name="depthValue">The current depth value</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The number of items per one page</param>
        /// <returns>Manager result with paged list orders</returns>
        public ManagerResult<SummaryOrdersDataInfoDto> GetOrdersSummaryDataByType(OrderTypeEnum orderType, decimal depthValue, int pageNumber, int pageSize)
        {
            try
            {
                return Ok(_ordersDataService.GetOrdersSummaryDataByType(orderType, depthValue, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                ExceptionsHandler.Handle(ex, $"Problem with getting orders data by type {orderType}.");
                return Error<SummaryOrdersDataInfoDto>(ex.Message);
            }
        }
    }
}
