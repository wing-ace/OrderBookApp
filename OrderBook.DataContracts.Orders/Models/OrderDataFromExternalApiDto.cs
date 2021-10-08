using Newtonsoft.Json;

namespace OrderBook.DataContracts.Orders.Models
{
    /// <summary>
    /// The data contract object
    /// that describes order information from external API
    /// </summary>
    public class OrderDataFromExternalApiDto
    {
        /// <summary>
        /// The order type
        /// </summary>
        [JsonProperty("OrderType")]
        public string OrderType { get; set; }

        /// <summary>
        /// The order price
        /// </summary>
        [JsonProperty("Price")]
        public decimal Price { get; set; }

        /// <summary>
        /// The current volume of asset/currency
        /// </summary>
        [JsonProperty("Volume")]
        public decimal Volume { get; set; }
    }
}
