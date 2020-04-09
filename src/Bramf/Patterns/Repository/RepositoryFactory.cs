using System;
using System.Collections.Generic;
using System.Linq;

namespace Bramf.Patterns.Repository
{
    /// <summary>
    /// The repository factory implementation
    /// </summary>
    internal class RepositoryFactory : IRepositoryFactory
    {
        #region Members

        private IList<IRepository> mRepositories;
        private bool mDisposed;

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public RepositoryFactory(IEnumerable<IRepository> repositories)
        {
            mRepositories = new List<IRepository>(repositories);
        }

        #region Methods

        /// <inheritdoc/>
        public IRepository<TEntity> Load<TEntity>(string name)
            where TEntity : IEntity
        {
            throwExceptionIfDisposed();

            IRepository repository = mRepositories.FirstOrDefault(x => x.Name == name);

            if (repository != null)
                if (repository is IRepository<TEntity> parsed)
                    return parsed;
                else
                    throw new ArgumentException($"Repository named '{name}' found. But its type is valid.");

            throw new ArgumentException($"Repository named '{name}' not found.");
        }

        /// <inheritdoc/>
        public IRepository<TEntity> Load<TEntity>()
            where TEntity : IEntity
        {
            throwExceptionIfDisposed();

            IRepository repository = mRepositories.FirstOrDefault(x => x.GetType() == typeof(IRepository<TEntity>));

            if (repository == null)
                throw new ArgumentException($"Repository of type {typeof(TEntity)} not found.");

            return repository as IRepository<TEntity>;
        }

        /// <summary>
        /// Disposes all the repository implementations
        /// </summary>
        public void Dispose()
        {
            throwExceptionIfDisposed();

            foreach(IRepository repository in mRepositories)
                repository.Dispose();

            mRepositories = null;
        }

        #endregion

        #region Helper Methods

        private void throwExceptionIfDisposed()
        {
            if (mDisposed)
                throw new ObjectDisposedException("Repository factory is disposed.");
        }

        #endregion
    }
}