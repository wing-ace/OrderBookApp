using Microsoft.AspNetCore.Mvc;
using OrderBook.BLL.OrdersData.Managers;

namespace OrderBookWebApp.Controllers
{
    /// <summary>
    /// The controller for get the order book's data
    /// </summary>
    public class OrderBookController : BaseContentController
    {
        private readonly OrdersDataManager _ordersDataManager;

        public OrderBookController([FromServices] OrdersDataManager ordersDataManager)
        {
            _ordersDataManager = ordersDataManager;
        }

        /// <summary>
        /// Get the sell orders data
        /// </summary>
        /// <param name="depthValue">The current depth value</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The number of items per one page</param>
        /// <returns>The paged list of sell orders</returns>
        [HttpGet]
        public IActionResult GetSellOrdersData(decimal depthValue, int pageNumber, int pageSize)
            => TransferToControllerResult(_ordersDataManager.GetSellOrdersSummaryData(depthValue, pageNumber, pageSize));

        /// <summary>
        /// Get the buy orders data
        /// </summary>
        /// <param name="depthValue">The current depth value</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The number of items per one page</param>
        /// <returns>The paged list of buy orders</returns>
        [HttpGet]
        public IActionResult GetBuyOrdersData(decimal depthValue, int pageNumber, int pageSize)
            => TransferToControllerResult(_ordersDataManager.GetBuyOrdersSummaryData(depthValue, pageNumber, pageSize));
    }
}
