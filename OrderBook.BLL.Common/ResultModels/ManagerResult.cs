namespace OrderBook.BLL.Common.ResultModels
{
    /// <summary>
    /// The result of manager's method call
    /// </summary>
    public class ManagerResult
    {
        /// <summary>
        /// The result state.
        /// <remarks>
        /// Can be success or failed
        /// </remarks>>
        /// </summary>
        public ManagerResultStateEnum ResultState { get; set; }

        /// <summary>
        /// The flag for check error state
        /// </summary>
        public bool IsInErrorState => ResultState == ManagerResultStateEnum.Error;

        /// <summary>
        /// The error model if result is in error state
        /// </summary>
        public ManagerResultErrorModel ErrorModel { get; set; }
    }

    /// <summary>
    /// The overload for manager result with return value
    /// </summary>
    /// <typeparam name="TResult">The type of return value</typeparam>
    public class ManagerResult<TResult> : ManagerResult
    {
        /// <summary>
        /// The manager's result return value
        /// </summary>
        public TResult Result { get; set; }
    }
}
