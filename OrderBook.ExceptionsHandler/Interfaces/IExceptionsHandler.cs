using System;

namespace OrderBook.ExceptionsHandler.Interfaces
{
    /// <summary>
    /// The interface that provides a common exceptions handling mechanism
    /// </summary>
    public interface IExceptionsHandler
    {
        /// <summary>
        /// Handle exception
        /// </summary>
        /// <param name="exception">The exception instance</param>
        /// <param name="operationTitle">
        /// The operation title during which the exception was thrown
        /// </param>
        void Handle(Exception exception, string operationTitle);
    }
}
