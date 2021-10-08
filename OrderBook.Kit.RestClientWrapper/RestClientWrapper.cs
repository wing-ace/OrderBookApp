using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderBook.Kit.RestClientWrapper.Interfaces;
using RestSharp;

namespace OrderBook.Kit.RestClientWrapper
{
    /// <summary>
    /// The wrapper's implementation for <see cref="RestClient"/>
    /// </summary>
    internal class RestClientWrapper : RestClient, IRestClientWrapper
    {
        private readonly ILogger<RestClientWrapper> _logger;

        public RestClientWrapper(ILogger<RestClientWrapper> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Execute GET request to <see cref="baseUrl"/>
        /// with collection of query params (<see cref="queryParams"/>)
        /// </summary>
        /// <typeparam name="TResult">Type of result model</typeparam>
        /// <param name="baseUrl">The base URL for send GET request</param>
        /// <param name="queryParams">The collection of query params</param>
        /// <returns>The result model of type <see cref="TResult"/></returns>
        public TResult ExecuteGetResquest<TResult>(Uri baseUrl, List<KeyValuePair<string, object>> queryParams) 
            where TResult: class 
        {
            var request = CreateGetRestRequest(baseUrl, queryParams);
            var response = base.Execute(request);
            CheckResponseHasSuccessStatus(response);

            return JsonConvert.DeserializeObject<TResult>(response.Content);
        }

        /// <summary>
        /// Check that the response has success <see cref="HttpStatusCode"/> status code 
        /// <remarks>
        /// If the response status code is not success, then an exception will be thrown
        /// </remarks>
        /// </summary>
        /// <param name="response">The response object</param>
        private void CheckResponseHasSuccessStatus(IRestResponse response)
        {
            if (response.IsSuccessful)
                return;

            var errorMessage =
                $"Request to the url {response.ResponseUri} failed. Response code is {response.StatusCode}. Details: {response.ErrorMessage}";

            _logger.LogError(errorMessage);
            throw new InvalidOperationException(errorMessage);
        }

        /// <summary>
        /// Create a <see cref="IRestRequest"/> object with params for send GET request
        /// </summary>
        /// <param name="baseUrl">The base URL for send GET request</param>
        /// <param name="queryParams">The collection of query params</param>
        /// <returns>Object of type <see cref="IRestRequest"/></returns>
        private IRestRequest CreateGetRestRequest(Uri baseUrl, List<KeyValuePair<string, object>> queryParams)
        {
            var request = new RestRequest(Method.GET);
            queryParams.ForEach(param => request.AddParameter(param.Key, param.Value));
            BaseUrl = baseUrl;

            return request;
        }
    }
}
