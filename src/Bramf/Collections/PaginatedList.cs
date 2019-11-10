using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bramf.Collections
{
    /// <summary>
    /// A collection that implements pagination system
    /// </summary>
    public class PaginatedList<T> : List<T>
    {
        #region Public Properties

        /// <summary>
        /// The current page index
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// The total pages count
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// A boolean that indicates if we have a previous page
        /// </summary>
        public bool HasPreviousPage => PageIndex > 1;

        /// <summary>
        /// A boolean that indicates if we have next pages
        /// </summary>
        public bool HasNextPage => PageIndex < TotalPages;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            // Calculate values
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            // Add the items to the list collection
            AddRange(items);
        }

        #endregion
    
        #region Methods

        /// <summary>
        /// Creates a new <see cref="PaginatedList{T}"/>
        /// </summary>
        /// <param name="source">The items source</param>
        /// <param name="pageIndex">The page index</param>
        /// <param name="pageSize">The size of each pages</param>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            // Get the amount of items the source has
            var count = await source.CountAsync();

            // Get items following pagination
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            // Return the created paginated list
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        #endregion
    }
}
