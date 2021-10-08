using System.Collections.Generic;
using System.Linq;

namespace OrderBook.CommonTools.Extensions
{
    /// <summary>
    /// The extensions for <see cref="IEnumerable{T}"/>
    /// </summary>
    public static class ExtensionsForIEnumerable
    {
        /// <summary>
        /// Get a set of elements based on <see cref="pageNumber"/> and <see cref="pageSize"/>
        /// </summary>
        /// <typeparam name="TModel">The type of list element</typeparam>
        /// <param name="recordsList">The list of records</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The current page size</param>
        /// <returns>
        /// The set of elements based on <see cref="pageNumber"/>
        /// and <see cref="pageSize"/>
        /// </returns>
        public static List<TModel> ToPagedList<TModel>(this IEnumerable<TModel> recordsList, int pageNumber, int pageSize)
        {
            var itemsToSkip = (pageNumber - 1) * pageSize;
            return recordsList.Skip(itemsToSkip).Take(pageSize).ToList();
        }
    }
}
