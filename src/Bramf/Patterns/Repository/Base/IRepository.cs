using Bramf.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bramf.Patterns.Repository
{
    /// <summary>
    /// Represents a repository
    /// </summary>
    public interface IRepository : IDisposable
    {
        /// <summary>
        /// The name of the repository
        /// </summary>
        string Name { get; set; }
    }

    /// <summary>
    /// Represents a repository of data
    /// </summary>
    /// <typeparam name="TEntity">The entity type to be stored.</typeparam>
    public interface IRepository<TEntity> : IRepository
        where TEntity : IEntity
    {
        #region Methods

        /// <summary>
        /// Adds an entity to the repository
        /// </summary>
        /// <param name="entity">The entity type.</param>
        void Add(TEntity entity);

        /// <summary>
        /// Removes an entity from the repository
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Updates an entity in the repository
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Returns a <see cref="IQueryable"/> of all the entities in the repository
        /// </summary>
        IQueryable<TEntity> Query();

        /// <summary>
        /// Finds an entity executing an expression
        /// </summary>
        /// <param name="expression">The expression to execute.</param>
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Finds many entities executing an expression
        /// </summary>
        /// <param name="expression">The expression to execute.</param>
        Task<IEnumerable<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Saves the changes made in the repository
        /// </summary>
        Task<OperationResult> SaveChangesAsync();

        #endregion
    }
}