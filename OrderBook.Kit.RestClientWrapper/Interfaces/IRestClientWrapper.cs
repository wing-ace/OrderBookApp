using System;
using System.Collections.Generic;

namespace OrderBook.Kit.RestClientWrapper.Interfaces
{
    /// <summary>
    /// The wrapper for <see cref="RestSharp.RestClient"/>
    /// </summary>
    public interface IRestClientWrapper
    {
        /// <summary>
        /// Execute GET request to <see cref="baseUrl"/>
        /// with collection of query params (<see cref="queryParams"/>)
        /// </summary>
        /// <typeparam name="TResult">Type of result model</typeparam>
        /// <param name="baseUrl">The base URL for send GET request</param>
        /// <param name="queryParams">The collection of query params</param>
        /// <returns>The result model of type <see cref="TResult"/></returns>
        TResult ExecuteGetResquest<TResult>(Uri baseUrl, List<KeyValuePair<string, object>> queryParams)
            where TResult : class;
    }
}
