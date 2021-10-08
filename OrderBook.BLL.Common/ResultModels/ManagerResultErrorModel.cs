namespace OrderBook.BLL.Common.ResultModels
{
    /// <summary>
    /// The error model for <see cref="ManagerResultStateEnum.Error"/> state
    /// </summary>
    public class ManagerResultErrorModel
    {
        /// <summary>
        /// The information about error
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The information for debug some error cases.
        /// <remarks>
        /// Can be null
        /// </remarks>
        /// </summary>
        public string DegubInfo { get; set; }
    }
}
