namespace OrderBook.DataContracts.Common.Models
{
    /// <summary>
    /// The base abstract data transfer object with information about pagination
    /// </summary>
    public abstract class DataResultWithPaginationDto
    {
        /// <summary>
        /// The current page number
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// The total items count in initial collection
        /// </summary>
        public int TotalItemsCount { get; set; }
    }
}
