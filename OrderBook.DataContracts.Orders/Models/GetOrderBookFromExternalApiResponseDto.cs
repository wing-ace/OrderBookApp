using System.Collections.Generic;
using Newtonsoft.Json;

namespace OrderBook.DataContracts.Orders.Models
{
    /// <summary>
    /// The data transfer object that
    /// represent the repsonse to external API with information about the order book
    /// </summary>
    public class GetOrderBookFromExternalApiResponseDto
    {
        public GetOrderBookFromExternalApiResponseDto()
        {
            BuyOrdersData = new List<OrderDataFromExternalApiDto>();
            SellOrdersData = new List<OrderDataFromExternalApiDto>();
        }

        /// <summary>
        /// The collection of buy orders
        /// </summary>
        [JsonProperty("BuyOrders")]
        public List<OrderDataFromExternalApiDto> BuyOrdersData { get; set; }

        /// <summary>
        /// The collection of sell orders
        /// </summary>
        [JsonProperty("SellOrders")]
        public List<OrderDataFromExternalApiDto> SellOrdersData { get; set; }
    }
}
