using System;

namespace OrderBook.Services.RefreshOrdersDataBackgroudService.Constants
{
    /// <summary>
    /// The constant values for fetch data from external API
    /// </summary>
    public static class FetchOrdersDataConstants
    {
        /// <summary>
        /// The external API URL
        /// </summary>
        public static readonly Uri ExternalApiUri = new Uri("https://api.independentreserve.com/Public/GetOrderBook");

        /// <summary>
        /// The query param name that represents a primary currency code
        /// </summary>
        public const string PrimaryCurrencyCodeParamName = "primaryCurrencyCode";

        /// <summary>
        /// The primary currency code value
        /// </summary>
        public const string PrimaryCurrencyCodeParamValue = "xbt";

        /// <summary>
        /// The query param name that represents a secondary currency code
        /// </summary>
        public const string SecondaryCurrencyCodeParamName = "secondaryCurrencyCode";

        /// <summary>
        /// The secondary currency code value
        /// </summary>
        public const string SecondaryCurrencyCodeParamValue = "aud";
    }
}
