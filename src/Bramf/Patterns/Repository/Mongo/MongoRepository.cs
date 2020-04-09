using Bramf.Extensions;
using Bramf.Operation;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bramf.Patterns.Repository.Mongo
{
    /// <summary>
    /// Represents a repository that stores data in a Mongo database
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class MongoRepository<TEntity> : IRepository<TEntity>
        where TEntity : IEntity
    {
        #region Properties

        /// <summary>
        /// The name of the repository
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Construction

        private readonly IMongoCollection<TEntity> mCollection;
        private readonly IList<IOperation> mPendingOperations;
        private bool mDisposed;

        /// <summary>
        /// Creates a new <see cref="MongoRepository{TEntity}"/>
        /// </summary>
        public MongoRepository(MongoRepositoryOptions options)
        {
            if (options.DatabaseName.IsNullOrWhitespace()) throw new ArgumentNullException(nameof(options.DatabaseName));
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (options.RepositoryName.IsNullOrWhitespace())
                Name = typeof(TEntity).Name;
            else
                Name = options.RepositoryName;

            var client = new MongoClient(options.ConnectionString);
            var database = client.GetDatabase(options.DatabaseName);
            mPendingOperations = new List<IOperation>();

            // Get the collection
            mCollection = database.GetCollection<TEntity>(getCollectionName(), new MongoCollectionSettings
            {
                AssignIdOnInsert = true,
            });
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        public void Add(TEntity entity)
        {
            throwExceptionIfDisposed();

            mPendingOperations.Add(new InsertOperation<TEntity>(entity, mCollection));
        }

        /// <inheritdoc/>
        public void Remove(TEntity entity)
        {
            throwExceptionIfDisposed();

            mPendingOperations.Add(new RemoveOperation<TEntity>(entity, mCollection));
        }

        /// <inheritdoc/>
        public void Update(TEntity entity)
        {
            throwExceptionIfDisposed();

            mPendingOperations.Add(new UpdateOperation<TEntity>(entity, mCollection));
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> Query()
            => mCollection.AsQueryable();

        /// <inheritdoc/>
        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
        {
            throwExceptionIfDisposed();

            var cursor = await mCollection.FindAsync(expression);
            return await cursor.FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> expression)
        {
            throwExceptionIfDisposed();

            var cursor = await mCollection.FindAsync(expression);
            return await cursor.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<OperationResult> SaveChangesAsync()
        {
            throwExceptionIfDisposed();

            var errors = new List<OperationError>();

            foreach (var operation in mPendingOperations)
            {
                var result = await operation.ExecuteAsync();

                if (!result.Succeeded)
                    errors.AddRange(result.Errors);
            }

            // Clear pending operations
            mPendingOperations.Clear();

            return OperationResult.Failed(errors.ToArray());
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            throwExceptionIfDisposed();

            mPendingOperations.Clear();
            mDisposed = true;
        }

        #endregion

        #region Helper Methods

        private string getCollectionName()
        {
            Type entityType = typeof(TEntity);

            // Try to get collection name from attribute
            var collectionNameAttribute = entityType.GetCustomAttributes(typeof(CollectionNameAttribute), false).FirstOrDefault();

            // Return name if attribute is present
            if (collectionNameAttribute != null)
                return (collectionNameAttribute as CollectionNameAttribute).CollectionName;

            // Otherwise, return the name of the entity type plus "s"
            return entityType.Name + "s";
        }

        private void throwExceptionIfDisposed()
        {
            if (mDisposed)
                throw new ObjectDisposedException("Repository closed.");
        }

        #endregion
    }
}