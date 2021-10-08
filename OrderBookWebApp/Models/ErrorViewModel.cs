namespace OrderBookWebApp.Models
{
    /// <summary>
    /// The view model with information about error
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// The current request id
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// The flag representing the need to show the id of the current request
        /// <remarks>
        /// The value is true if the field <see cref="RequestId"/> is set
        /// </remarks>
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
