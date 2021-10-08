using Microsoft.AspNetCore.Mvc;
using OrderBook.BLL.OrdersData.Managers;
using OrderBook.DataContracts.Orders.Enums;

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
        /// Get the orders summary data based on order type
        /// </summary>
        /// <param name="orderType">The type of order (<see cref="OrderTypeEnum"/>)</param>
        /// <param name="depthValue">The current depth value</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The number of items per one page</param>
        /// <returns>The paged list of orders</returns>
        [HttpGet]
        public IActionResult GetOrdersDataByType(OrderTypeEnum orderType, decimal depthValue, int pageNumber,
            int pageSize)
            => TransferToControllerResult(_ordersDataManager.GetOrdersSummaryDataByType(orderType, depthValue, pageNumber, pageSize));
    }
}
