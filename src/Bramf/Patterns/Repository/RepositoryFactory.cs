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

        private IDictionary<string, Type> mRepositories;
        private IServiceProvider mServices;

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public RepositoryFactory(IServiceProvider services, IEnumerable<IRepository> repositories)
        {
            mServices = services;
            mRepositories = new Dictionary<string, Type>();

            foreach (IRepository repository in repositories)
                mRepositories.Add(repository.Name, repository.GetType());
        }

        #region Methods

        /// <inheritdoc/>
        public IRepository<TEntity> Load<TEntity>(string name)
            where TEntity : IEntity
        {
            if(!mRepositories.ContainsKey(name))
                throw new ArgumentException($"Repository named '{name}' not found.");

            // Get repository definition
            Type repositoryDefinition = mRepositories[name];

            // Get repository from services
            object repository = mServices.GetService(repositoryDefinition);

            // Not valid repository type
            if(repository == null)
                throw new ArgumentException($"Repository named '{name}' found but its type is not valid.");

            // Cast and return
            return (IRepository<TEntity>)repository;
        }

        /// <inheritdoc/>
        public IRepository<TEntity> Load<TEntity>()
            where TEntity : IEntity
        {
            try
            {
                // Get repository definition
                Type repositoryDefinition = mRepositories.Select(x => x.Value).Where(x => x.GetGenericArguments().Any(x => x == typeof(TEntity))).FirstOrDefault();

                // Get repository from services
                object repository = mServices.GetService(repositoryDefinition);

                // Not valid repository type
                if (repository == null)
                    throw new ArgumentException($"Repository with store type '{typeof(TEntity)}' not found.");

                // Cast and return
                return (IRepository<TEntity>)repository;
            }
            catch
            {
                throw new ArgumentException($"Repository with store type '{typeof(TEntity)}' not found.");
            }
        }

        #endregion
    }
}