using OrderBook.BLL.Common.ResultModels;
using OrderBook.ExceptionsHandler.Interfaces;

namespace OrderBook.BLL.Common
{
    /// <summary>
    /// The base abstract class of manager
    /// </summary>
    public abstract class BaseManager
    {
        protected readonly IExceptionsHandler ExceptionsHandler;

        protected BaseManager(IExceptionsHandler exceptionsHandler)
        {
            ExceptionsHandler = exceptionsHandler;
        }

        /// <summary>
        /// The success result of manager's method execution
        /// </summary>
        /// <returns>
        /// The <see cref="ManagerResult"/>
        /// with <see cref="ManagerResultStateEnum.Ok"/> state
        /// </returns>
        protected ManagerResult Ok()
            => CreateManagerResultObj(ManagerResultStateEnum.Ok);

        /// <summary>
        /// The success result of manager's method execution
        /// </summary>
        /// <typeparam name="TResult">The type of return value</typeparam>
        /// <param name="result">The result of method execution</param>
        /// <returns>
        /// The <see cref="ManagerResult{TResult}"/>
        /// with <see cref="ManagerResultStateEnum.Ok"/> state
        /// </returns>
        protected ManagerResult<TResult> Ok<TResult>(TResult result)
            => CreateManagerResultObj(result, ManagerResultStateEnum.Ok);

        /// <summary>
        /// The error result of manager's method execution
        /// </summary>
        /// <param name="errorMessage">The information about error</param>
        /// <param name="debugInfo">The information for debug some error cases</param>
        /// <returns>
        /// The <see cref="ManagerResult"/>
        /// with <see cref="ManagerResultStateEnum.Error"/> state
        /// </returns>
        protected ManagerResult Error(string errorMessage, string debugInfo = null)
            => CreateManagerResultObj(ManagerResultStateEnum.Error,
                CreateManagerResultErrorModel(errorMessage, debugInfo));

        /// <summary>
        /// The error result of manager's method execution
        /// </summary>
        /// <typeparam name="TResult">The type of return value</typeparam>
        /// <param name="errorMessage">The information about error</param>
        /// <param name="debugInfo">The information for debug some error cases</param>
        /// <returns>
        /// The <see cref="ManagerResult{TResult}"/>
        /// with <see cref="ManagerResultStateEnum.Error"/> state
        /// </returns>
        protected ManagerResult<TResult> Error<TResult>(string errorMessage, string debugInfo = null)
            => CreateManagerErrorResultObj<TResult>(CreateManagerResultErrorModel(errorMessage, debugInfo));

        /// <summary>
        /// Create object of type <see cref="ManagerResult"/>
        /// </summary>
        /// <param name="resultState">The result state of type <see cref="ManagerResultStateEnum"/></param>
        /// <param name="errorModel">The model with information about error</param>
        /// <returns>The object of type <see cref="ManagerResult"/></returns>
        private static ManagerResult CreateManagerResultObj(ManagerResultStateEnum resultState,
            ManagerResultErrorModel errorModel = null)
            => new ManagerResult
            {
                ResultState = resultState,
                ErrorModel = errorModel
            };

        /// <summary>
        /// Create object of type <see cref="ManagerResult{TResult}"/>
        /// </summary>
        /// <param name="result">The result of method execution</param>
        /// <param name="resultState">The result state of type <see cref="ManagerResultStateEnum"/></param>
        /// <param name="errorModel">The model with information about error</param>
        /// <returns>The object of type <see cref="ManagerResult{TResult}"/></returns>
        private static ManagerResult<TResult> CreateManagerResultObj<TResult>(TResult result, ManagerResultStateEnum resultState,
            ManagerResultErrorModel errorModel = null)
            => new ManagerResult<TResult>
            {
                Result = result,
                ResultState = resultState,
                ErrorModel = errorModel
            };

        /// <summary>
        /// Create object of type <see cref="ManagerResultErrorModel"/>
        /// </summary>
        /// <param name="errorMessage">The information about error</param>
        /// <param name="debugInfo">The information for debug some error cases</param>
        /// <returns>The object of type <see cref="ManagerResultErrorModel"/></returns>
        private static ManagerResultErrorModel CreateManagerResultErrorModel(string errorMessage, string debugInfo = null)
            => new ManagerResultErrorModel
            {
                ErrorMessage = errorMessage,
                DegubInfo = debugInfo
            };

        /// <summary>
        /// Create error object of type <see cref="ManagerResult{TResult}"/>
        /// </summary>
        /// <param name="errorModel">The model with information about error</param>
        /// <returns>The error object of type <see cref="ManagerResult{TResult}"/></returns>
        private static ManagerResult<TResult> CreateManagerErrorResultObj<TResult>(
            ManagerResultErrorModel errorModel = null)
            => new ManagerResult<TResult>
            {
                ResultState = ManagerResultStateEnum.Error,
                ErrorModel = errorModel
            };
    }
}
