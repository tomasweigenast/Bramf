using Bramf.Extensions;
using Bramf.Operation;
using Bramf.Patterns.Repository.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bramf.Patterns.Repository
{
    /// <summary>
    /// Represents a repository that stores data in a database
    /// </summary>
    /// <typeparam name="TEntity">The entity type to be stored.</typeparam>
    /// <typeparam name="TContext">The context type to use.</typeparam>
    public class EFRepository<TEntity, TContext> : IRepository<TEntity>
        where TContext : DbContext
        where TEntity : class, IEntity
    {
        #region Properties

        /// <summary>
        /// The name of the repository
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Construction

        private readonly TContext mContext;

        /// <summary>
        /// Creates a new <see cref="EFRepository{TEntity, TContext}"/>
        /// </summary>
        /// <param name="context">The context must be filled up with dependency injection.</param>
        /// <param name="options">The repository options.</param>
        public EFRepository(TContext context, EFRepositoryOptions options)
        {
            if (options.Name.IsNullOrWhitespace())
                Name = typeof(TEntity).Name;
            else
                Name = options.Name;

            // Instantiate context
            mContext = context;

            if (!ExistsDbSet())
                throw new InvalidOperationException($"Type {typeof(TEntity)} not found in {typeof(TContext)} DbContext.");
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        public void Add(TEntity entity)
            => mContext.Add(entity);

        /// <inheritdoc/>
        public IQueryable<TEntity> Query()
            => mContext.Set<TEntity>();

        /// <inheritdoc/>
        public void Remove(TEntity entity)
            => mContext.Remove(entity);

        /// <inheritdoc/>
        public void Update(TEntity entity)
            => mContext.Update(entity);

        /// <inheritdoc/>
        public async Task<OperationResult> SaveChangesAsync()
        {
            try
            {
                await mContext.SaveChangesAsync();
                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                return OperationResult.Failed(ex.Message);
            }
        }

        /// <inheritdoc/>
        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
            => await mContext.Set<TEntity>().Where(expression).FirstOrDefaultAsync();

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> expression)
            => await mContext.Set<TEntity>().Where(expression).ToListAsync();

        /// <summary>
        /// Disposes the current instance
        /// </summary>
        public void Dispose()
        {
            if (mContext != null)
                mContext.Dispose();
        }

        #endregion

        #region Helper Methods

        private bool ExistsDbSet()
        {
            var metadata = mContext.Model.FindEntityType(typeof(TEntity));

            if (metadata == null)
                return false;

            return true;
        }

        private TContext InstantiateContext()
        {
            var parameterlessConstructor = typeof(TContext).GetConstructor(new Type[] { });
            if (parameterlessConstructor == null)
                throw new ArgumentException($"A default context can only be created if the context type '{typeof(TContext)}' has a parameterless constructor.");

            return Activator.CreateInstance<TContext>();
        }

        #endregion
    }
}
