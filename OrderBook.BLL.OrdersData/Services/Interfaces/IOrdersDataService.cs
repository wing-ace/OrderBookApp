using OrderBook.DataContracts.Orders.Models;

namespace OrderBook.BLL.OrdersData.Services.Interfaces
{
    /// <summary>
    /// The interface for get summary order's data
    /// </summary>
    public interface IOrdersDataService
    {
        /// <summary>
        /// Get the summary sell order's data
        /// </summary>
        /// <param name="depthValue">The current depth value</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The number of items per one page</param>
        /// <returns>The paged list of sell orders</returns>
        SummaryOrdersDataInfoDto GetSellOrdersSummaryData(decimal depthValue, int pageNumber, int pageSize);

        /// <summary>
        /// Get the summary buy order's data
        /// </summary>
        /// <param name="depthValue">The current depth value</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The number of items per one page</param>
        /// <returns>The paged list of buy orders</returns>
        SummaryOrdersDataInfoDto GetBuyOrdersSummaryData(decimal depthValue, int pageNumber, int pageSize);
    }
}
