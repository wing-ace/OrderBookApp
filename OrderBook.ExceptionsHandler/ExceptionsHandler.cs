using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using OrderBook.ExceptionsHandler.Interfaces;

namespace OrderBook.ExceptionsHandler
{
    /// <summary>
    /// The implementation of <see cref="IExceptionsHandler"/> interface
    /// </summary>
    internal class ExceptionsHandler : IExceptionsHandler
    {
        private readonly ILogger<ExceptionsHandler> _logger;
        private readonly List<(Func<Exception, bool> CheckExceptionType, Action<Exception, string> Handle)> _mapExceptionTypeToHandleActionList;

        public ExceptionsHandler(ILogger<ExceptionsHandler> logger)
        {
            _logger = logger;
            _mapExceptionTypeToHandleActionList =
                new List<(Func<Exception, bool> CheckExceptionType, Action<Exception, string> Handle)>
                {
                    (ex => ex.GetType() == typeof(Exception), HandleCriticalException),
                    (ex => ex is InvalidOperationException, HandleCriticalException),
                    (ex => ex is NotImplementedException, HandleWarning),
                    (ex => ex is ApplicationException, HandleCriticalException),
                    (ex => ex is NullReferenceException, HandleCriticalException),
                    (ex => ex is DivideByZeroException, HandleWarning)
                };
        }

        /// <summary>
        /// Handle exception
        /// </summary>
        /// <param name="exception">The exception instance</param>
        /// <param name="operationTitle">
        /// The operation title during which the exception was thrown
        /// </param>
        public void Handle(Exception exception, string operationTitle)
        {
            var handleExceptionFunc =
                _mapExceptionTypeToHandleActionList.FirstOrDefault(checkFunc =>
                    checkFunc.CheckExceptionType(exception));

            if (handleExceptionFunc == default)
            {
                return;
            }

            handleExceptionFunc.Handle(exception, operationTitle);
        }

        /// <summary>
        /// The mock of handling critical exceptions
        /// </summary>
        /// <param name="exception">The exception instance</param>
        /// <param name="operationTitle">
        /// The operation title during which the exception was thrown
        /// </param>
        private void HandleCriticalException(Exception exception, string operationTitle)
        {
            _logger.LogError(
                $"It is a critical exception with operation \"{operationTitle}\". " +
                "You must implement some logic for register this event on your bug tracking system. " +
                $"Exception is \"{exception.Message}\"");
        }

        /// <summary>
        /// The mock of handling noncritical exceptions
        /// </summary>
        /// <param name="exception">The exception instance</param>
        /// <param name="operationTitle">
        /// The operation title during which the exception was thrown
        /// </param>
        private void HandleWarning(Exception exception, string operationTitle)
        {
            _logger.LogError(
                $"It is a exception with operation \"{operationTitle}\". " +
                "It can be handled by sending email on support or writing to log file " +
                $"Exception is \"{exception.Message}\"");
        }
    }
}
