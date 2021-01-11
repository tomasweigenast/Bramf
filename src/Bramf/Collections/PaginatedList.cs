using Microsoft.EntityFrameworkCore;
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
        /// The current page
        /// </summary>
        public int CurrentPage { get; }

        /// <summary>
        /// A boolean that indicates if we have a previous page
        /// </summary>
        public bool HasPreviousPage => CurrentPage > 1;

        /// <summary>
        /// A boolean that indicates if we have next pages
        /// </summary>
        public bool HasNextPage { get; }

        /// <summary>
        /// The page size
        /// </summary>
        public int PageSize { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public PaginatedList(List<T> items, int pageIndex, int pageSize)
        {
            PageSize = pageSize;
            CurrentPage = pageIndex;
            HasNextPage = items.Count == pageSize;

            // Add the items to the list collection
            AddRange(items);
        }

        #endregion
    
        #region Methods

        /// <summary>
        /// Creates a new <see cref="PaginatedList{T}"/> paging asynchronously the results
        /// of an <see cref="IQueryable{T}"/>
        /// </summary>
        /// <param name="source">The items source</param>
        /// <param name="page">The page index</param>
        /// <param name="pageSize">The size of each pages</param>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int page, int pageSize)
        {
            // Get items following pagination
            List<T> items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // Return the created paginated list
            return new PaginatedList<T>(items, page, pageSize);
        }

        /// <summary>
        /// Creates a new <see cref="PaginatedList{T}"/> paging the results
        /// of an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="source">The items source</param>
        /// <param name="page">The page index</param>
        /// <param name="pageSize">The size of each pages</param>
        public static PaginatedList<T> Create(IEnumerable<T> source, int page, int pageSize)
        {
            // Get items following pagination
            List<T> items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Return the created paginated list
            return new PaginatedList<T>(items, page, pageSize);
        }

        #endregion
    }

    /// <summary>
    /// Extension methods to create <see cref="PaginatedList{T}"/>s 
    /// </summary>
    public static class PaginatedListExtensions
    {
        /// <summary>
        /// Paginates the result of an <see cref="IQueryable{T}"/>
        /// </summary>
        /// <param name="source">The items source</param>
        /// <param name="page">The page index</param>
        /// <param name="pageSize">The size of each pages</param>
        public static Task<PaginatedList<T>> PaginateAsync<T>(this IQueryable<T> source, int page, int pageSize)
            => PaginatedList<T>.CreateAsync(source, page, pageSize);

        /// <summary>
        /// Paginates the result of an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="source">The items source</param>
        /// <param name="page">The page index</param>
        /// <param name="pageSize">The size of each pages</param>
        public static PaginatedList<T> Paginate<T>(this IEnumerable<T> source, int page, int pageSize)
            => PaginatedList<T>.Create(source, page, pageSize);
    }
}