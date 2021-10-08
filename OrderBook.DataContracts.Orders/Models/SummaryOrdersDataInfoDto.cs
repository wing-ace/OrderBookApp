using System.Collections.Generic;
using OrderBook.DataContracts.Common.Models;

namespace OrderBook.DataContracts.Orders.Models
{
    /// <summary>
    /// The data transfer object with summary order's data and pagination
    /// </summary>
    public class SummaryOrdersDataInfoDto : DataResultWithPaginationDto
    {
        public SummaryOrdersDataInfoDto()
        {
            OrdersList = new List<OrderInfoDc>();
        }

        /// <summary>
        /// The list of orders for current page
        /// </summary>
        public List<OrderInfoDc> OrdersList { get; set; }
    }
}
