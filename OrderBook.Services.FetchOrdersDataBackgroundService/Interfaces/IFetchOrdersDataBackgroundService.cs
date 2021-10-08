using System.Collections.Generic;
using OrderBook.DataContracts.Orders.Models;

namespace OrderBook.Services.FetchOrdersDataBackgroundService.Interfaces
{
    /// <summary>
    /// The interface for background service for fetch orders data from external API
    /// </summary>
    public interface IFetchOrdersDataBackgroundService
    {
        /// <summary>
        /// Get the list of sell orders
        /// </summary>
        /// <returns>The list of sell orders</returns>
        IEnumerable<OrderDataFromExternalApiDto> GetSellOrdersData();

        /// <summary>
        /// Get the list of buy orders
        /// </summary>
        /// <returns>The list of buy orders</returns>
        IEnumerable<OrderDataFromExternalApiDto> GetBuyOrdersData();
    }
}
