namespace OrderBook.DataContracts.Orders.Models
{
    public class OrderInfoDc
    {
        /// <summary>
        /// The order price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The volume of asset/currency
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// The multiplication of Price and Volume, calculated from best price to worst
        /// </summary>
        public decimal CumulativeVolume { get; set; }
    }
}
