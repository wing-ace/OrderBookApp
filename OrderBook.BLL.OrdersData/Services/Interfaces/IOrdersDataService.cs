using OrderBook.DataContracts.Orders.Enums;
using OrderBook.DataContracts.Orders.Models;

namespace OrderBook.BLL.OrdersData.Services.Interfaces
{
    /// <summary>
    /// The interface for get summary order's data
    /// </summary>
    public interface IOrdersDataService
    {
        /// <summary>
        /// Get the summary orders data
        /// </summary>
        /// <param name="orderType">The type of order (<see cref="OrderTypeEnum"/>)</param>
        /// <param name="depthValue">The current depth value</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The number of items per one page</param>
        /// <returns>The paged list of buy orders</returns>
        SummaryOrdersDataInfoDto GetOrdersSummaryDataByType(OrderTypeEnum orderType, decimal depthValue, int pageNumber, int pageSize);
    }
}
